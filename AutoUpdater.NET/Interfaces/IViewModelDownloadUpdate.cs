// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using AutoUpdaterDotNET.Models;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelDownloadUpdate
{
    DownloadStatistics      ProgressPercentage { get; set; }
    // ReSharper disable once InconsistentNaming
    CancellationTokenSource? CTS               { get; }

    Task DownloadUpdate();
}