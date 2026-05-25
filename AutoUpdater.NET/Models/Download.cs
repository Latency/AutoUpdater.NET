// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.Methods.cs
// Author:   Latency McLaughlin
// Date:     01/23/2026
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AssemblyLoader;
using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Properties;
using AutoUpdaterDotNET.Views;
using FluentFTP;
using Microsoft.ExceptionMessageBox.MessageBox;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Threading;
using AutoUpdaterDotNET.Converters;
using WindowService.ViewModels;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Timers.Timer;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Main class that lets you auto update applications by setting some fields and executing its Start method.
/// </summary>
public sealed class Download : IDownload
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public Download(Window_Restricted? window = null)
    {
        _window = window;

        TimerNodeList[0].Items.AddDelegate(null);
        UpdateCompleteNodeList[0].Items.AddDelegate(null);
        BeforeCheckForUpdatesNodeList[0].Items.AddDelegate(null);
        AfterCheckForUpdatesNodeList[0].Items.AddDelegate(null);
    }

    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private                 Timer?                   _remindLaterTimer;
    private readonly        DispatcherTimer          _updateTimer = new();
    private readonly        Assembly                 _assembly    = Assembly.GetExecutingAssembly();
    private                 CancellationTokenSource? _ftpCTS;
    private readonly        Progress<FtpProgress>    _ftpProgress = new();
    private readonly        Window_Restricted?       _window;

    private static readonly JsonSerializerOptions _options = new()
    {
        ReadCommentHandling = JsonCommentHandling.Skip,
        Converters = {
            new ConfigInitializationConverter()
        }
    };
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public event EventHandler UpdateTimer
    {
        add
        {
            _updateTimer.Tick += value;
            TimerNodeList[0].Items.Add(value);
        }
        remove
        {
            _updateTimer.Tick -= value;
            TimerNodeList[0].Items.Remove(value);
        }
    }

    private event Action? _updateComplete;
    public event Action UpdateComplete
    {
        add
        {
            _updateComplete += value;
            UpdateCompleteNodeList[0].Items.Add(value);
        }
        remove
        {
            _updateComplete -= value;
            UpdateCompleteNodeList[0].Items.Remove(value);
        }
    }

    private event Action? _beforeCheckForUpdates;
    public event Action BeforeCheckForUpdates
    {
        add
        {
            _beforeCheckForUpdates += value;
            BeforeCheckForUpdatesNodeList[0].Items.Add(value);
        }
        remove
        {
            _beforeCheckForUpdates -= value;
            BeforeCheckForUpdatesNodeList[0].Items.Remove(value);
        }
    }

    private event Action<UpdateInfo>? _afterCheckForUpdates;
    public event Action<UpdateInfo> AfterCheckForUpdates
    {
        add
        {
            _afterCheckForUpdates += value;
            AfterCheckForUpdatesNodeList[0].Items.AddDelegate(value);
        }
        remove
        {
            _afterCheckForUpdates -= value;
            AfterCheckForUpdatesNodeList[0].Items.RemoveDelegate(value);
        }
    }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public ObservableCollection<TreeViewItem> TimerNodeList { get; init; } = [new TreeViewItem().Header(nameof(TimerNodeList))];


    public ObservableCollection<TreeViewItem> UpdateCompleteNodeList { get; init; } = [new TreeViewItem().Header(nameof(UpdateCompleteNodeList))];


    public ObservableCollection<TreeViewItem> BeforeCheckForUpdatesNodeList { get; init; } = [new TreeViewItem().Header(nameof(BeforeCheckForUpdatesNodeList))];



    public ObservableCollection<TreeViewItem> AfterCheckForUpdatesNodeList { get; init; } = [new TreeViewItem().Header(nameof(AfterCheckForUpdates))];


    public IConfig Config { get; set; }


    // ReSharper disable once InconsistentNaming
    public CancellationTokenSource? CTS { get; private set; }



    public bool IsRunning { get; set; }


    /// <summary>
    ///     Set Basic Authentication credentials required to download the XML file.
    /// </summary>
    public AuthenticationHeaderValue? BasicAuthHeaderValue { get; set; }


    internal Uri BaseAddress { get; init; }

    internal AsyncFtpClient AsyncFtpClient
    {
        get
        {
            if (Config.FtpProfile is null || string.IsNullOrEmpty(Config.FtpProfile.Host) || Config.FtpProfile.Credentials is null)
                throw new NullReferenceException();

            var client = new AsyncFtpClient(Config.FtpProfile.Host, Config.FtpProfile.Credentials.UserName, Config.FtpProfile.Credentials.Password);

            // Recommended: Auto-detect encryption and accept any server certificate for simplicity
            client.Config!.EncryptionMode         = FtpEncryptionMode.Auto;
            client.Config!.ValidateAnyCertificate = true;

            return client;
        }
    }

    internal HttpClient HttpWebClient
    {
        get
        {
            var httpClientHandler = new HttpClientHandler
            {
                Credentials             = CredentialCache.DefaultCredentials as NetworkCredential,
                PreAuthenticate         = true,
                AllowAutoRedirect       = true,
                MaxConnectionsPerServer = 1,
                UseCookies              = false,
                AutomaticDecompression  = DecompressionMethods.GZip,
                UseDefaultCredentials   = true,
                UseProxy                = Config.ProxyEnabled,
                Proxy                   = Config.ProxyEnabled ? new WebProxy { Address = new Uri(Config.ProxyUri ?? throw new NullReferenceException(nameof(Config.ProxyUri))) } : null,
                DefaultProxyCredentials = Config.ProxyEnabled ? new NetworkCredential(Config.ProxyUserName, Config.ProxyPassword) : new CredentialCache()
            };
            return new(httpClientHandler)
            {
                BaseAddress = BaseAddress,
                DefaultRequestHeaders =
                {
                    Authorization = BasicAuthHeaderValue
                }
            };
        }
    }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    /// <summary>
    ///     Start checking for new version of application and display a dialog to the user if update is available.
    /// </summary>
    public async Task Start()
    {
        if (Config.IsMandatory && _remindLaterTimer != null)
        {
            _remindLaterTimer.Stop();
            _remindLaterTimer.Dispose();
            _remindLaterTimer = null;
        }

        if (IsRunning || _remindLaterTimer != null)
            return;

        var task = CheckUpdate();

        try
        {
            IsRunning = true;

            if (Config.CheckSynchronously)
            {
                var updateInfo = await task;
                await StartUpdate(updateInfo);
            }
            else
            {
                try
                {
                    _ = task.ContinueWith(async t =>
                    {
                        if (t.IsCanceled)
                            throw new TaskCanceledException();

                        await StartUpdate(t);
                    });
                }
                catch (TaskCanceledException)
                {
                    // Handled
                }
            }
        }
        catch (Exception exception)
        {
            var win = new ExceptionMessageBox(exception);
            win.Show(_window!.Handle, exception.Message, "Exception in Start");
        }
        finally
        {
            IsRunning = false;
        }
    }


    private async Task<UpdateInfo?> CheckUpdate()
    {
        _beforeCheckForUpdates?.Invoke();

        if (string.IsNullOrEmpty(Config.AppTitle))
            Config.AppTitle = _assembly.Title() ?? _assembly.GetName().Name!;

        var configFile = $@"{Directory.GetCurrentDirectory()}\Properties\{Settings.Default!.ConfigFile!.Decrypt(Settings.Default.CipherKey!)}";
        var json = await File.ReadAllTextAsync(configFile);
        Config = JsonSerializer.Deserialize<Config>(json, _options)!;

        if (BaseAddress.Scheme.Equals(Uri.UriSchemeFtp))
        {
            _ftpCTS = new CancellationTokenSource();

            try
            {
                using var client = new FtpClient(Config.FtpProfile!.Host!, Config.FtpProfile!.Credentials!.UserName, Config.FtpProfile.Credentials.Password);
                client.Connect();

                var workingDirectory = $"/public_html/{Settings.Default.RemotePath!.Decrypt(Settings.Default.CipherKey!)}";
                client.SetWorkingDirectory(workingDirectory);
                // Download the file into a memory stream
                using var ms = new MemoryStream();
                if (client.DownloadStream(ms, Settings.Default.UpdateInfoFile!.Decrypt(Settings.Default.CipherKey!)))
                {
                    // Reset stream position and read as string
                    ms.Position = 0;
                    using var reader = new StreamReader(ms);
                    json = await reader.ReadToEndAsync();
                    var o = JsonSerializer.Deserialize<UpdateInfo>(json, _options);
                    if (o is null)
                        throw new Exception("Failed to deserialize update information from FTP server.");
                    o.BaseAddress = BaseAddress;
                    o.DownloadURL = $"{BaseAddress.OriginalString}{workingDirectory}/{Settings.Default.UpdateInfoFile!.Decrypt(Settings.Default.CipherKey!)}";
                }
            }
            catch (TaskCanceledException)
            {
                // Handled
            }
            finally
            {
                await AsyncFtpClient.Disconnect(_ftpCTS.Token);

                if (_ftpCTS is not null)
                {
                    _ftpCTS.Dispose();
                    _ftpCTS = null;
                }
            }
        }
        else
        {
            using var response = await HttpWebClient.GetAsync($"{BaseAddress}{Settings.Default!.RemotePath!.Decrypt(Settings.Default.CipherKey!)}/{Settings.Default.UpdateInfoFile!.Decrypt(Settings.Default.CipherKey!)}");

            if (!response.IsSuccessStatusCode)
            {
                ShowError(new HttpRequestException(response.ReasonPhrase));
                return null;
            }

            json = await response.Content.ReadAsStringAsync();
        }

        // Check if the JSON file was read properly.
        if (string.IsNullOrEmpty(json))
        {
            Trace.WriteLine("Unable to read JSON configuration file.");
            return null;
        }

        var args = JsonSerializer.Deserialize<UpdateInfo>(json, _options);
        if (args is null)
            return null;

        args.BaseAddress = BaseAddress;
        args.DownloadURL = $"{BaseAddress}{Settings.Default!.RemotePath!.Decrypt(Settings.Default.CipherKey!)}/{Settings.Default.ConfigFile!.Decrypt(Settings.Default.CipherKey!)}";

        // Invoke Invocator
        _afterCheckForUpdates?.Invoke(args);

        return args;
    }


    /// <summary>
    ///     StartUpdate
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private async Task StartUpdate(object? result)
    {
        switch (result)
        {
            case DateTime time:
            {
                var timeSpan = time - DateTime.Now;

                _remindLaterTimer = new Timer
                {
                    Interval  = Math.Max(1, timeSpan.TotalMilliseconds),
                    AutoReset = false
                };

                _remindLaterTimer.Elapsed += delegate
                {
                    _remindLaterTimer = null;
                    Start().RunSynchronously(TaskScheduler.Current);
                };

                _remindLaterTimer.Start();
                break;
            }
            case UpdateInfo ui:
            {
                if (_window!.DataContext is not ViewModelRestricted vm)
                    throw new NullReferenceException();

                // Calculate
                var checkSum = new CheckSum(_assembly.GetFile(_assembly.FullName!)!, ui.CheckSum.HashingAlgorithm);
                if (checkSum.HashValue != ui.CheckSum.HashValue)
                {
                    if (Config is { IsMandatory: true, UpdateMode: Mode.ForcedDownload })
                    {
                        var vmDlUpdate = (IViewModelDownloadUpdate) vm;
                        await DownloadUpdate(ui, vmDlUpdate.ProgressHandler, vmDlUpdate.ContentCallback);
                    }
                    else
                    {
                        var dlWin = vm.Dependencies.WindowService.InitializeWindow<Window_Update, IViewModelUpdate>(_window);

                        dlWin.LabelTitle!.Content    = string.Format(dlWin.LabelTitle.Tag!.ToString()!, ui.Version);
                        dlWin.LabelDescription!.Text = string.Format(dlWin.LabelDescription.Tag!.ToString()!, _assembly.Version(), ui.Version);

                        if (Config.WindowSize is not null)
                        {
                            dlWin.Width  = Config.WindowSize.Width;
                            dlWin.Height = Config.WindowSize.Height;
                        }

                        dlWin.Show();
                    }

                    return;
                }

                if (Config.ReportErrors)
                    MessageBox.Show(_window?.Owner!, Settings.Default!.UpdateUnavailableMessage!, Settings.Default.UpdateUnavailableCaption!, MessageBoxButton.OK, MessageBoxImage.Information);
                break;
            }
        }
    }


    /// <summary>
    ///     DownloadUpdate
    /// </summary>
    /// <returns></returns>
    private async Task DownloadUpdate(UpdateInfo updateInfo, IProgress<double>? progressHandler, Action<long, long>? contentCallback = null)
    {
        try
        {
            var tempFile = Path.GetTempFileName();

            // Create a file stream to store the downloaded data.
            // This really can be any type of writeable stream.
            await using var stream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None);
            CTS = new CancellationTokenSource();
            await HttpWebClient.DownloadAsync(updateInfo.DownloadURL!, stream, progressHandler, contentCallback, CTS.Token);

            if (Config.CheckSum is null)
                throw new Exception(Settings.Default!.FileIntegrityCheckFailedMessage);

            var hash = new CheckSum(stream).HashValue;
            if (hash != null && hash.Equals(Config.CheckSum.HashValue, StringComparison.OrdinalIgnoreCase))
                return;

            var tempPath = Path.Combine(Path.GetTempPath(), tempFile);

            if (File.Exists(tempPath))
                File.Delete(tempPath);

            File.Move(tempFile, tempPath);

            string? installerArgs = null;
            if (!string.IsNullOrEmpty(updateInfo.InstallerArgs))
                installerArgs = updateInfo.InstallerArgs.Replace("%path%", Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName));

            var processStartInfo = new ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true,
                Arguments = installerArgs ?? string.Empty
            };

            var extension = Path.GetExtension(tempPath);
            if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                var localPath = !string.IsNullOrEmpty(Config.ExecutablePathOverride?.Path) ? Config.ExecutablePathOverride.Path : _assembly.Location;

                if (Directory.Exists(localPath))
                {
                    if (Config.ClearAppDirectory)
                    {
                        // Ensure the destination directory does not exist or handle overwrites carefully
                        // The method can create the directory if it doesn't exist.
                        Directory.Delete(localPath, true); // Deletes the directory and its contents
                    }

                    await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(tempPath, localPath);
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

                // Event Invocator
                _updateComplete?.Invoke();
            }
            catch (Win32Exception exception)
            {
                if (exception.NativeErrorCode != 1223 /* ERROR_CANCELLED */)
                    throw;
            }
        }
        catch (TaskCanceledException)
        {
            // Handled
        }
        catch (Exception e)
        {
            MessageBox.Show(_window?.Owner!, e.Message, e.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _window?.Owner?.Close();
        }
    }


    /// <summary>
    ///     ShowError
    /// </summary>
    /// <param name="exception"></param>
    private void ShowError(Exception exception)
    {
        if (!Config.ReportErrors)
            return;

        if (exception is WebException)
            MessageBox.Show(Settings.Default!.UpdateCheckFailedMessage!, Settings.Default.UpdateCheckFailedCaption!, MessageBoxButton.OK, MessageBoxImage.Error);
        else
            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
    }
}