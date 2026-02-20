// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Globalization;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Converters;
using AutoUpdaterDotNET.Properties;
using Microsoft.Extensions.DependencyInjection;
using WindowService.ViewModels;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelDownloadUpdate : ViewModelRestricted, IViewModelDownloadUpdate
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelDownloadUpdate(BaseServiceDependencies dependencies) : base(dependencies)
    {
        _singletonDownload = new(() => new Download(dependencies.ServiceProvider.GetRequiredService<IViewModelConfig>().Config, Window as Window_Restricted));

        ((IViewModelDownloadUpdate)this).ProgressBarCallback += OnProgressBarChanged;
        ((IViewModelDownloadUpdate)this).ContentCallback     += OnContentChanged;
        ((IViewModelDownloadUpdate)this).ProgressHandler     =  new Progress<double>(((IViewModelDownloadUpdate)this).ProgressBarCallback);
    }


    #region Static Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private static readonly Lazy<BytesToStringConverter> SingletonConverter = new(() => new BytesToStringConverter());
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Static Fields


    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private readonly Lazy<Download> _singletonDownload;
    private          DateTime       _startedAt;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public Download Download => _singletonDownload.Value;

    [ObservableProperty]
    public partial string? LabelInformation { get; set; }


    [ObservableProperty]
    public partial DownloadStatistics DownloadStatistics { get; set; } = new();

    Progress<double>? IViewModelDownloadUpdate.  ProgressHandler     { get; set; }
    Action<double>? IViewModelDownloadUpdate.    ProgressBarCallback { get; set; }
    Action<long, long>? IViewModelDownloadUpdate.ContentCallback     { get; set; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public async Task Start()
    {
        _startedAt = default;
        await _singletonDownload.Value.Start();
    }


    private void OnProgressBarChanged(double value) => DownloadStatistics.ProgressPercentage = value;


    private void OnContentChanged(long bytesReceived, long totalBytesToReceive)
    {
        DownloadStatistics.BytesReceived       = bytesReceived;
        DownloadStatistics.TotalBytesToReceive = totalBytesToReceive;

        if (_startedAt == default)
            _startedAt = DateTime.Now;
        else
        {
            var timeSpan     = DateTime.Now - _startedAt;
            var totalSeconds = (long)timeSpan.TotalSeconds;
            if (totalSeconds > 0)
            {
                var bytesPerSecond = bytesReceived / totalSeconds;
                LabelInformation = string.Format(Settings.Default!.DownloadSpeedMessage!, SingletonConverter.Value.Convert(bytesPerSecond, bytesPerSecond.GetType(), null, CultureInfo.CurrentCulture));
            }
        }
    }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}