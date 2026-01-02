// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_DownloadUpdate.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Enums;

namespace AutoUpdaterDotNET.Views;

public sealed partial class Window_DownloadUpdate : RestrictedWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_DownloadUpdate()
    {
        InitializeComponent();

        ControlBox = !Window_AutoUpdater.Instance.Mandatory || Window_AutoUpdater.Instance.UpdateMode != Mode.ForcedDownload;
    }
}