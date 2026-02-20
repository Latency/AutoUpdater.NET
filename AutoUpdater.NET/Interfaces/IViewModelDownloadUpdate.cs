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
    Download            Download            { get; }
    DownloadStatistics  DownloadStatistics  { get; set; }
    Progress<double>?   ProgressHandler     { get; set; }
    Action<double>?     ProgressBarCallback { get; set; }
    Action<long, long>? ContentCallback     { get; set; }

    Task Start();
}