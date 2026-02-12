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
using AutoUpdaterDotNET.Views;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using AutoUpdaterDotNET.Properties;
using WindowService.Interfaces;
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
    private void DownloadUpdate(UpdateInfoEventArgs args) => UpdateComplete?.Invoke();


    /// <summary>
    ///     Shows standard update dialog.
    /// </summary>
    private void ShowUpdateForm(UpdateInfoEventArgs args)
    {
        var window = Dependencies.WindowService.InitializeWindow<Window_Update, IViewModelUpdate>(((IViewModelRestricted)this).Owner);

        window.LabelTitle!.Content    = string.Format(window.LabelTitle.Tag!.ToString()!,       args.InstalledVersion);
        window.LabelDescription!.Text = string.Format(window.LabelDescription.Tag!.ToString()!, args.CurrentVersion, args.InstalledVersion);

        if (args.WindowSize.HasValue)
        {
            window.Width  = args.WindowSize.Value.Width;
            window.Height = args.WindowSize.Value.Height;
        }

        window.Show();
    }


    /// <summary>
    ///     Start checking for new version of application and display a dialog to the user if update is available.
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="myAssembly">Assembly to use for version checking.</param>
    public async Task Start(string domain, Assembly? myAssembly = null)
    {
        if (Config.IsMandatory && _remindLaterTimer != null)
        {
            _remindLaterTimer.Stop();
            _remindLaterTimer.Dispose();
            _remindLaterTimer = null;
        }

        if (Running || _remindLaterTimer != null)
            return;

        try
        {
            _baseUri  = new Uri(Path.Combine(domain, Settings.Default!.ConfigFile!));
            _assembly = myAssembly ?? Assembly.GetEntryAssembly()!;

            await Start();
        }
        catch (Exception ex)
        {
            ShowError(ex);
        }
        finally
        {
            HttpWebClient.Dispose();
        }
    }


    private async Task Start()
    {
        try
        {
            Running = true;
            if (Config.CheckSynchronously)
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
        if (string.IsNullOrEmpty(Config.AppTitle))
            Config.AppTitle = _assembly.Title() ?? _assembly.GetName().Name!;

        using var response = await HttpWebClient.GetAsync(_baseUri);
        if (!response.IsSuccessStatusCode)
        {
            //var a = JsonSerializer.Serialize(new UpdateInfoEventArgs(), new JsonSerializerOptions { WriteIndented = true });
            //await File.WriteAllTextAsync($@"K:\{Settings.Default!.ConfigFile}", a);

            ShowError(new HttpRequestException(response.ReasonPhrase));
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(json))
            throw new Exception("The JSON is required to handle the ParseUpdateInfoEvent when url is not specified.");

        var args = JsonSerializer.Deserialize<UpdateInfoEventArgs>(json);

        if (string.IsNullOrEmpty(args?.CurrentVersion?.ToString()))
        {
            MessageBox.Show($"{nameof(args.CurrentVersion)} must be defined!", Settings.Default!.UpdateUnavailableCaption!, MessageBoxButton.OK, MessageBoxImage.Stop);
            return args;
        }

        if (string.IsNullOrEmpty(args.CurrentVersion?.ToString()) || string.IsNullOrEmpty(args.DownloadURL))
            throw new MissingFieldException();

        args.InstalledVersion = new Version2
        {
            Version = _assembly.GetName().Version
        };

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
        }
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
                    if (Config is { IsMandatory: true, UpdateMode: Mode.ForcedDownload })
                        DownloadUpdate(args);
                    else
                        ShowUpdateForm(args);

                    return true;
                }

                if (Config.ReportErrors)
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
            if (!Config.ReportErrors)
                return;

            if (exception is WebException)
                MessageBox.Show(Settings.Default!.UpdateCheckFailedMessage!, Settings.Default.UpdateCheckFailedCaption!, MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}