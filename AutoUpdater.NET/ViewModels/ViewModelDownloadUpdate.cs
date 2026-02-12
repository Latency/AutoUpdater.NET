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
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WindowService.Interfaces;
using WindowService.ViewModels;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelDownloadUpdate : ViewModelRestricted, IViewModelDownloadUpdate
{
    /*
 *    {
       var args = new UpdateInfoEventArgs
       {
           InstalledVersion = new Version2
           {
               Version = new Version(1, 0, 0, 0)
           },
           CurrentVersion = new Version2
           {
               Version = new Version(2, 0, 0, 0)
           }
       };
       _configVm.ShowUpdateForm(args);
   }

 */

    #region Fields

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private DateTime _startedAt;

    private readonly UpdateInfoEventArgs _args;
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
    public ViewModelDownloadUpdate(BaseServiceDependencies dependencies) : base(dependencies)
    {
        _progressHandler = new Progress<double>(((IViewModelDownloadUpdate)this).ProgressBarCallback);
    }


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    // ReSharper disable once InconsistentNaming
    public CancellationTokenSource? CTS { get; private set; }

    [ObservableProperty]
    public partial DownloadStatistics DownloadStatistics { get; set; } = new();

    Action<double> IViewModelDownloadUpdate.ProgressBarCallback { get; set; } = delegate { };
    Action<long, long> IViewModelDownloadUpdate.ContentCallback { get; set; } = delegate { };

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    internal static void Wnd_Loaded(Window wnd)
    {
        if (wnd.DataContext is ViewModelDownloadUpdate dc)
            ((IViewModelRestricted)dc).Owner = wnd;
    }


    /// <summary>
    ///     DownloadUpdate
    /// </summary>
    /// <returns></returns>
    public async Task DownloadUpdate(IConfig? config)
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
            await ViewModelConfig.HttpWebClient.DownloadAsync(_args.DownloadURL, file, _progressHandler, ((IViewModelDownloadUpdate)this).ContentCallback, CTS.Token);
            WebClientOnDownloadFileCompleted(tempFile);
        }
        catch (TaskCanceledException e)
        {
            // Handled
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

            var vm = Dependencies.ServiceProvider.GetRequiredService<IViewModelConfig>();
            var config = vm.Config;

            var extension = Path.GetExtension(tempPath);
            if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(config.InstallationPath) && Directory.Exists(config.InstallationPath))
                {
                    if (config.ClearAppDirectory)
                    {
                        // Ensure the destination directory does not exist or handle overwrites carefully
                        // The method can create the directory if it doesn't exist.
                        Directory.Delete(config.InstallationPath, true); // Deletes the directory and its contents
                    }

                    System.IO.Compression.ZipFile.ExtractToDirectory(tempPath, config.InstallationPath);
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

            if (config.RunUpdateAsAdmin)
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
            MessageBox.Show(((IViewModelRestricted)this).Owner!, e.Message, e.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            ((IViewModelRestricted)this).Owner?.Close();
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