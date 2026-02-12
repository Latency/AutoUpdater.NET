// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using AutoUpdaterDotNET.Models;
using WindowService.Interfaces;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelDownloadUpdate : IViewModelRestricted
{
    Task DownloadUpdate(IConfig config);

    DownloadStatistics DownloadStatistics { get; set; }

    // ReSharper disable once InconsistentNaming
    CancellationTokenSource? CTS          { get; }

    Action<double> ProgressBarCallback  { get; set; }

    Action<long, long> ContentCallback { get; set; }
}