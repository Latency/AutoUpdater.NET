// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_DownloadUpdate.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.ViewModels;
using System.ComponentModel;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Views;

public sealed partial class Window_DownloadUpdate : Window_Restricted
{
    private readonly IViewModelConfig         _vmConfig;
    private readonly IViewModelDownloadUpdate _vmDownloadUpdate;


    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_DownloadUpdate(IViewModelConfig vmConfig, IViewModelDownloadUpdate vmDownloadUpdate)
    {
        InitializeComponent();

        _vmConfig         = vmConfig;
        _vmDownloadUpdate = vmDownloadUpdate;
    }


    private void OnClosing(object? sender, CancelEventArgs e)
    {
        if (_vmConfig is not { IsMandatory: true, UpdateMode: Mode.ForcedDownload })
            ViewModelConfig.HttpWebClient.CancelPendingRequests();

        _vmDownloadUpdate.CTS?.Cancel();
        e.Cancel = true;
    }
}