using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Windows;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using Microsoft.VisualBasic.CompilerServices;

namespace AutoUpdaterDotNET;

internal partial class DownloadUpdateDialog : Window
{
    private readonly UpdateInfoEventArgs _args;

    private DateTime _startedAt;

    private string _tempFile;

    public DownloadUpdateDialog(UpdateInfoEventArgs args)
    {
        InitializeComponent();
        TopMost = AutoUpdater.TopMost;

        if (AutoUpdater.Icon != null)
        {
            Icon = Icon.FromHandle(AutoUpdater.Icon.GetHicon());
        }

        _args = args;
    }

    private void DownloadUpdateDialogLoad(object sender, EventArgs e)
    {
        var uri = new Uri(_args.DownloadURL);

        _webClient = AutoUpdater.GetWebClient(uri, AutoUpdater.BasicAuthDownload);

        if (string.IsNullOrEmpty(AutoUpdater.DownloadPath))
        {
            _tempFile = Path.GetTempFileName();
        }
        else
        {
            _tempFile = Path.Combine(AutoUpdater.DownloadPath, $"{Guid.NewGuid().ToString()}.tmp");
            if (!Directory.Exists(AutoUpdater.DownloadPath))
            {
                Directory.CreateDirectory(AutoUpdater.DownloadPath);
            }
        }

        _webClient.DownloadProgressChanged += OnDownloadProgressChanged;

        _webClient.DownloadFileCompleted += WebClientOnDownloadFileCompleted;

        _webClient.DownloadFileAsync(uri, _tempFile);
    }

    private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        if (_startedAt == default)
        {
            _startedAt = DateTime.Now;
        }
        else
        {
            var timeSpan = DateTime.Now - _startedAt;
            var totalSeconds = (long)timeSpan.TotalSeconds;
            if (totalSeconds > 0)
            {
                var bytesPerSecond = e.BytesReceived / totalSeconds;
                labelInformation.Text = string.Format(Resources.DownloadSpeedMessage, BytesToString(bytesPerSecond));
            }
        }

        labelSize.Text = $@"{BytesToString(e.BytesReceived)} / {BytesToString(e.TotalBytesToReceive)}";
        progressBar.Value = e.ProgressPercentage;
    }

    private void WebClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
    {
        if (asyncCompletedEventArgs.Cancelled)
        {
            return;
        }

        try
        {
            if (asyncCompletedEventArgs.Error != null)
            {
                throw asyncCompletedEventArgs.Error;
            }

            if (_args.CheckSum != null)
            {
                CompareChecksum(_tempFile, _args.CheckSum);
            }

            // Try to parse the content disposition header if it exists.
            ContentDisposition? contentDisposition = null;
            if (!string.IsNullOrWhiteSpace(_webClient.ResponseHeaders?["Content-Disposition"]))
            {
                try
                {
                    contentDisposition =
                        new ContentDisposition(_webClient.ResponseHeaders["Content-Disposition"]);
                }
                catch (FormatException)
                {
                    // Ignore content disposition header if it is wrongly formatted.
                    contentDisposition = null;
                }
            }

            var fileName = string.IsNullOrEmpty(contentDisposition?.FileName)
                               ? Path.GetFileName(_webClient.ResponseUri.LocalPath)
                               : contentDisposition.FileName;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new WebException(Resources.UnableToDetermineFilenameMessage);
            }

            var tempPath =
                Path.Combine(
                    string.IsNullOrEmpty(AutoUpdater.DownloadPath)
                        ? Path.GetTempPath()
                        : AutoUpdater.DownloadPath,
                    fileName);

            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

            File.Move(_tempFile, tempPath);

            string? installerArgs = null;
            if (!string.IsNullOrEmpty(_args.InstallerArgs))
            {
                installerArgs = _args.InstallerArgs.Replace("%path%",
                    Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName));
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true,
                Arguments = installerArgs ?? string.Empty
            };

            var extension = Path.GetExtension(tempPath);
            if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                var installerPath =
                    Path.Combine(Path.GetDirectoryName(tempPath) ?? throw new InvalidOperationException(),
                        "ZipExtractor.exe");

                File.WriteAllBytes(installerPath, Resources.ZipExtractor);

                var currentExe = Process.GetCurrentProcess().MainModule?.FileName;
                var updatedExe = _args.ExecutablePath;
                var extractionPath = Path.GetDirectoryName(currentExe);

                if (string.IsNullOrWhiteSpace(updatedExe) &&
                    !string.IsNullOrWhiteSpace(AutoUpdater.ExecutablePath))
                {
                    updatedExe = AutoUpdater.ExecutablePath;
                }

                if (!string.IsNullOrWhiteSpace(AutoUpdater.InstallationPath) &&
                    Directory.Exists(AutoUpdater.InstallationPath))
                {
                    extractionPath = AutoUpdater.InstallationPath;
                }

                processStartInfo = new ProcessStartInfo
                {
                    FileName = installerPath,
                    UseShellExecute = true
                };

                var arguments = new Collection<string>
                {
                    "--input",
                    tempPath,
                    "--output",
                    extractionPath,
                    "--current-exe",
                    currentExe
                };

                if (!string.IsNullOrWhiteSpace(updatedExe))
                {
                    arguments.Add("--updated-exe");
                    arguments.Add(updatedExe);
                }

                if (AutoUpdater.ClearAppDirectory)
                {
                    arguments.Add("--clear");
                }

                var args = Environment.GetCommandLineArgs();
                if (args.Length > 1)
                {
                    arguments.Add("--args");
                    arguments.Add(string.Join(" ", args.Skip(1).Select(arg => $"\"{arg}\"")));
                }

                processStartInfo.Arguments = Utils.BuildArguments(arguments);
            }
            else if (extension.Equals(".msi", StringComparison.OrdinalIgnoreCase))
            {
                processStartInfo = new ProcessStartInfo
                {
                    FileName = "msiexec",
                    Arguments = $"/i \"{tempPath}\""
                };

                if (!string.IsNullOrEmpty(installerArgs))
                {
                    processStartInfo.Arguments += $" {installerArgs}";
                }
            }

            if (AutoUpdater.RunUpdateAsAdmin)
            {
                processStartInfo.Verb = "runas";
            }

            try
            {
                Process.Start(processStartInfo);
            }
            catch (Win32Exception exception)
            {
                if (exception.NativeErrorCode == 1223)
                {
                    _webClient = null;
                }
                else
                {
                    throw;
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(this, e.Message, e.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            _webClient = null;
        }
        finally
        {
            DialogResult = _webClient == null ? DialogResult.Cancel : DialogResult.OK;
            FormClosing -= DownloadUpdateDialog_FormClosing;
            Close();
        }
    }

    private static string BytesToString(long byteCount)
    {
        string[] suf = ["B", "KB", "MB", "GB", "TB", "PB", "EB"];
        if (byteCount == 0)
        {
            return "0" + suf[0];
        }

        var bytes = Math.Abs(byteCount);
        var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        var num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
    }

    private static void CompareChecksum(string fileName, CheckSum checksum)
    {
        HashAlgorithm hashAlgorithm;
        if (string.IsNullOrEmpty(checksum.HashingAlgorithm) || checksum.HashingAlgorithm == "MD5")
        {
            hashAlgorithm = MD5.Create();
        }
        else
        {
            hashAlgorithm = checksum.HashingAlgorithm switch
            {
                "SHA1" => SHA1.Create(),
                "SHA256" => SHA256.Create(),
                "SHA384" => SHA384.Create(),
                "SHA512" => SHA512.Create(),
                _ => throw new NotSupportedException(Resources.HashAlgorithmNotSupportedMessage)
            };
        }

        using var stream = File.OpenRead(fileName);

        var hash = hashAlgorithm.ComputeHash(stream);
        var fileChecksum = Convert.ToHexStringLower(hash);

        if (fileChecksum.Equals(checksum.Value, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        throw new Exception(Resources.FileIntegrityCheckFailedMessage);
    }

    private void DownloadUpdateDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (AutoUpdater.Mandatory && AutoUpdater.UpdateMode == Mode.ForcedDownload)
        {
            AutoUpdater.Exit();
            return;
        }

        if (_webClient is not { IsBusy: true })
        {
            return;
        }

        _webClient.CancelAsync();
        DialogResult = DialogResult.Cancel;
    }
}