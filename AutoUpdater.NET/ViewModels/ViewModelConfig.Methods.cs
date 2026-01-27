// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.Methods.cs
// Author:   Latency McLaughlin
// Date:     01/23/2026
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AssemblyLoader;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Persistance_Providers;
using AutoUpdaterDotNET.Views;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using AutoUpdaterDotNET.Properties;
using Microsoft.Extensions.DependencyInjection;
using Timer = System.Timers.Timer;

namespace AutoUpdaterDotNET.ViewModels;

/// <summary>
///     Main class that lets you auto update applications by setting some fields and executing its Start method.
/// </summary>
public partial class ViewModelConfig
{
    /// <summary>
    ///     Opens the Download window that download the update and execute the installer when download completes.
    /// </summary>
    public void DownloadUpdate(UpdateInfoEventArgs args)
    {

        // Event Invocator
        UpdateComplete?.Invoke();
    }


    /// <summary>
    ///     Shows standard update dialog.
    /// </summary>
    public void ShowUpdateForm(UpdateInfoEventArgs args)
    {
        var _vmUpdate      = _serviceProvider.GetRequiredService<IViewModelUpdate>();
        var _windowService = _serviceProvider.GetRequiredService<IWindowService>();
        var window         = _windowService.InitializeWindow<Window_Update, IViewModelUpdate>(null, _vmUpdate);

        window.LabelTitle!.Content    = string.Format(window.LabelTitle.Tag!.ToString()!,       args.InstalledVersion);
        window.LabelDescription!.Text = string.Format(window.LabelDescription.Tag!.ToString()!, args.CurrentVersion, args.InstalledVersion);

        window.Show();
    }


    /// <summary>
    ///     Start checking for new version of application and display a dialog to the user if update is available.
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="myAssembly">Assembly to use for version checking.</param>
    public async Task Start(string domain, Assembly? myAssembly = null)
    {
        if (IsMandatory && _remindLaterTimer != null)
        {
            _remindLaterTimer.Stop();
            _remindLaterTimer.Close();
            _remindLaterTimer = null;
        }

        if (Running || _remindLaterTimer != null)
            return;


        try
        {
            var config = Settings.Default!.ConfigFile!;
            var str    = Path.Combine(domain, config);
            _baseUri  = new Uri(str);
            _assembly = myAssembly ?? Assembly.GetEntryAssembly()!;
        }
        catch (Exception ex)
        {
            ;
        }

        await Start();
    }


    private async Task Start()
    {
        try
        {
            Running = true;
            if (CheckSynchronously)
            {
                try
                {
                    var args = await CheckUpdate();
                    StartUpdate(args);
                }
                catch (Exception exception)
                {
                    ShowError(exception);
                }
            }
            else
            {
                try
                {
                    await CheckUpdate().ContinueWith(t =>
                       {
                           var args = t.Result;
                           if (args?.Error != null)
                               ShowError(args.Error);
                           else
                           {
                               if (!t.IsCanceled && StartUpdate(args))
                                   return;

                               Running = false;
                           }
                       })
                       .ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    // Handled
                }
                catch (Exception exception)
                {
                    ShowError(exception);
                }
            }
        }
        finally
        {
            Running = false;
        }
    }


    private async Task<UpdateInfoEventArgs?> CheckUpdate()
    {
        var appCompany = _assembly.Company();

        if (string.IsNullOrEmpty(AppTitle))
            AppTitle = _assembly.Title() ?? _assembly.GetName().Name!;

        var registryLocation = !string.IsNullOrEmpty(appCompany) ? $@"Software\{appCompany}\{AppTitle}\AutoUpdater" : $@"Software\{AppTitle}\AutoUpdater";
        PersistenceProvider = new Registry(registryLocation);

        using var response = await GetWebClient(_baseUri, BasicAuthHeaderValue);
        var json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(json))
            throw new Exception("It is required to handle the ParseUpdateInfoEvent when url is not specified.");

        UpdateInfoEventArgs? args;
        if (ParseUpdateInfo == null)
            args = JsonSerializer.Deserialize<UpdateInfoEventArgs>(json);
        else
        {
            var parseArgs = new ParseUpdateInfoEventArgs(json);

            // Event invocator
            ParseUpdateInfo?.Invoke(parseArgs);
            args = parseArgs;
        }

        if (string.IsNullOrEmpty(args?.CurrentVersion?.ToString()))
        {
            MessageBox.Show($"{nameof(args.CurrentVersion)} must be defined!", Settings.Default!.UpdateUnavailableCaption!, MessageBoxButton.OK, MessageBoxImage.Stop);
            return args;
        }

        if (string.IsNullOrEmpty(args.CurrentVersion?.ToString()) || string.IsNullOrEmpty(args.DownloadURL))
            throw new MissingFieldException();

        var ver = new Version2
        {
            Version = _assembly.GetName().Version
        };
        args.InstalledVersion  = ver;
        args.IsUpdateAvailable = args.CurrentVersion.Version > args.InstalledVersion.Version;

        if (!IsMandatory)
        {
            if (string.IsNullOrEmpty(args.Mandatory.MinimumVersion) || args.InstalledVersion.Version < new Version(args.Mandatory.MinimumVersion))
            {
                IsMandatory  = args.Mandatory.Value;
                UpdateMode = args.Mandatory.UpdateMode;
            }

            // Read the persisted state from the persistence provider.
            // This method makes the persistence handling independent from the storage method.
            var skippedVersion = PersistenceProvider.GetSkippedVersion();
            if (skippedVersion != null)
            {
                var currentVersion = args.CurrentVersion.Version;
                if (currentVersion <= skippedVersion)
                    return null;

                if (currentVersion > skippedVersion)
                    // Update the persisted state. Its no longer makes sense to have this flag set as we are working on a newer application version.
                    PersistenceProvider.SetSkippedVersion(null);
            }

            var remindLaterAt = PersistenceProvider.GetRemindLater();
            if (remindLaterAt == null)
                return args;

            if (DateTime.Compare(DateTime.Now, remindLaterAt.Value) < 0)
                args.TimeStamp = remindLaterAt.Value;
        }
        else
        {
            ShowRemindLaterButton = false;
            ShowSkipButton        = false;
        }

        return args;
    }


    /// <summary>
    ///     StartUpdate
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    private bool StartUpdate(object? result)
    {
        if (result is DateTime time)
            SetTimer(time);
        else
        {
            if (result is not UpdateInfoEventArgs args)
                return false;

            if (CheckForUpdates != null)
                CheckForUpdates?.Invoke(args);
            else
            {
                if (args.IsUpdateAvailable)
                {
                    if (IsMandatory && UpdateMode == Mode.ForcedDownload)
                        DownloadUpdate(args);
                    else
                        ShowUpdateForm(args);

                    return true;
                }

                if (ReportErrors)
                    MessageBox.Show(Settings.Default!.UpdateUnavailableMessage!, Settings.Default.UpdateUnavailableCaption!, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        return false;
    }


    /// <summary>
    ///     ShowError
    /// </summary>
    /// <param name="exception"></param>
    private void ShowError(Exception exception)
    {
        if (CheckForUpdates != null)
        {
            // Event Invocator
            CheckForUpdates.Invoke(new UpdateInfoEventArgs
            {
                Error = exception
            });
        }
        else
        {
            if (!ReportErrors)
                return;

            if (exception is WebException)
                MessageBox.Show(Settings.Default!.UpdateCheckFailedMessage!, Settings.Default.UpdateCheckFailedCaption!, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private void SetVersion()
    {
        if (!InstalledVersionOverride)
        {
            var defaultVersion = GetType().Assembly.Version()!;

            MajorVersion    = (ushort)defaultVersion.Major;
            MinorVersion    = (ushort)defaultVersion.Minor;
            BuildVersion    = (ushort)defaultVersion.Build;
            RevisionVersion = (ushort)defaultVersion.Revision;
        }

        base.Update();
    }


    private void SetTimer(DateTime remindLater)
    {
        var timeSpan = remindLater - DateTime.Now;

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
    }


    private Task<HttpResponseMessage> GetWebClient(Uri uri, AuthenticationHeaderValue basicAuthentication)
    {
        _baseUri                                           = uri;
        HttpWebClient.DefaultRequestHeaders.Authorization = basicAuthentication;
        return HttpWebClient.GetAsync(_baseUri);
    }


    /// <summary>
    ///     Set Proxy server to use for all the web requests in AutoUpdater.NET.
    /// </summary>
    private static void WebProxy(string username, string password, string address)
    {
        HttpClientHandlerInstance.Proxy = new WebProxy
        {
            Address = new Uri(address)
        };
        HttpClientHandlerInstance.DefaultProxyCredentials = new NetworkCredential(username, password);
        HttpClientHandlerInstance.UseProxy                = true;
    }
}