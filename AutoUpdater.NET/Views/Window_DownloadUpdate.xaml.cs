// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_DownloadUpdate.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Views;

public sealed partial class Window_DownloadUpdate : RestrictedWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="vm"></param>
    public Window_DownloadUpdate(IViewModelDownloadUpdate vm)
    {
        InitializeComponent();

        DataContext = vm;

        ControlBox = !Window_AutoUpdater.Instance.Mandatory || Window_AutoUpdater.Instance.UpdateMode != Mode.ForcedDownload;
    }
}