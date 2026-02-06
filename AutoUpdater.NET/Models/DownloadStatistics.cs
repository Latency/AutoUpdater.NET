// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     DownloadStatistics.cs
// Author:   Latency McLaughlin
// Date:     01/27/2026
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.Models;

public partial class DownloadStatistics : ObservableObject
{
    [ObservableProperty]
    public partial long BytesReceived        { get; set; }

    [ObservableProperty]
    public partial long TotalBytesToReceive  { get; set; }

    [ObservableProperty]
    public partial double ProgressPercentage { get; set; }
}