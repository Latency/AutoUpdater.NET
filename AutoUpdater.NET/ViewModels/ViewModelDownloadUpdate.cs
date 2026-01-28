// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Properties;
using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;

namespace AutoUpdaterDotNET.ViewModels;

public sealed partial class ViewModelDownloadUpdate : ViewModelRestricted, IViewModelDownloadUpdate
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private          DateTime              _startedAt;
    private readonly Window_DownloadUpdate _window;
    private readonly UpdateInfoEventArgs   _args;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    private readonly Progress<double> _progressHandler;

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelDownloadUpdate(IServiceProvider serviceProvider, Window_DownloadUpdate window, IViewModelConfig vmConfig) : base(serviceProvider, window, vmConfig)
    {
        _window = window;

        _progressHandler = new Progress<double>(value =>
        {
            // This code runs on the UI thread, allowing safe updates to the ProgressBar
            _window.ProgressBarDownload?.Value = value;

            // Optional: Update the UI immediately to avoid display delays
            _window.ProgressBarDownload?.UpdateLayout();
        });
    }


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    // ReSharper disable once InconsistentNaming
    public CancellationTokenSource? CTS { get; private set; }

    [ObservableProperty]
    public partial DownloadStatistics ProgressPercentage { get; set; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    /// <summary>
    ///     DownloadUpdate
    /// </summary>
    /// <returns></returns>
    public async Task DownloadUpdate()
    {
        string tempFile;

        if (string.IsNullOrEmpty(_args.DownloadPath))
            tempFile = Path.GetTempFileName();
        else
        {
            tempFile = Path.Combine(_args.DownloadPath, $"{Guid.NewGuid().ToString()}.tmp");
            if (!Directory.Exists(_args.DownloadPath))
                Directory.CreateDirectory(_args.DownloadPath);
        }

        // Create a file stream to store the downloaded data.
        // This really can be any type of writeable stream.
        await using var file = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None);

        try
        {
            CTS = new CancellationTokenSource();
            await ViewModelConfig.HttpWebClient.DownloadAsync(_args.DownloadURL, file, _progressHandler, ContentCallback, CTS.Token);
            WebClientOnDownloadFileCompleted(tempFile);
        }
        catch (TaskCanceledException e)
        {
            // Handled
        }

        return;

        void ContentCallback(long bytesReceived, long totalBytesToReceive)
        {
            if (_startedAt == default)
                _startedAt = DateTime.Now;
            else
            {
                var timeSpan     = DateTime.Now - _startedAt;
                var totalSeconds = (long)timeSpan.TotalSeconds;
                if (totalSeconds > 0)
                {
                    var bytesPerSecond = bytesReceived / totalSeconds;
                    _window.LabelInformation?.Content = string.Format(Settings.Default!.DownloadSpeedMessage!, BytesToString(bytesPerSecond));
                }
            }

            _window.LabelSize?.Content = $"{BytesToString(bytesReceived)} / {BytesToString(totalBytesToReceive)}";
        }


        static string BytesToString(long byteCount)
        {
            string[] suf = ["B", "KB", "MB", "GB", "TB", "PB", "EB"];
            if (byteCount == 0)
                return "0" + suf[0];

            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num   = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
        }
    }


    private void WebClientOnDownloadFileCompleted(string tempFile)
    {
        try
        {
            if (_args.CheckSum != null)
                CompareChecksum(tempFile, _args.CheckSum);

            var tempPath =
                Path.Combine(
                    string.IsNullOrEmpty(_args.DownloadPath)
                        ? Path.GetTempPath()
                        : _args.DownloadPath,
                    tempFile);

            if (File.Exists(tempPath))
                File.Delete(tempPath);

            File.Move(tempFile, tempPath);

            string? installerArgs = null;
            if (!string.IsNullOrEmpty(_args.InstallerArgs))
                installerArgs = _args.InstallerArgs.Replace("%path%", Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName));

            var processStartInfo = new ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true,
                Arguments = installerArgs ?? string.Empty
            };

            var extension = Path.GetExtension(tempPath);
            if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(Config.InstallationPath) && Directory.Exists(Config.InstallationPath))
                {
                    if (Config.ClearAppDirectory)
                    {
                        // Ensure the destination directory does not exist or handle overwrites carefully
                        // The method can create the directory if it doesn't exist.
                        Directory.Delete(Config.InstallationPath, true); // Deletes the directory and its contents
                    }

                    System.IO.Compression.ZipFile.ExtractToDirectory(tempPath, Config.InstallationPath);
                }
            }
            else if (extension.Equals(".msi", StringComparison.OrdinalIgnoreCase))
            {
                processStartInfo = new ProcessStartInfo
                {
                    FileName = "msiexec",
                    Arguments = $"/i \"{tempPath}\""
                };

                if (!string.IsNullOrEmpty(installerArgs))
                    processStartInfo.Arguments += $" {installerArgs}";
            }

            if (Config.RunUpdateAsAdmin)
                processStartInfo.Verb = "runas";

            try
            {
                Process.Start(processStartInfo);
            }
            catch (Win32Exception exception)
            {
                if (exception.NativeErrorCode == 1223 /* ERROR_CANCELLED */)
                    ;
                else
                    throw;
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(_window, e.Message, e.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _window.Close();
        }
    }


    private static void CompareChecksum(string fileName, CheckSum checksum)
    {
        using var stream = File.OpenRead(fileName);

        var fileChecksum = new CheckSum(stream).HashValue;
        if (fileChecksum.Equals(checksum.HashValue, StringComparison.OrdinalIgnoreCase))
            return;

        throw new Exception(Settings.Default!.FileIntegrityCheckFailedMessage);
    }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}