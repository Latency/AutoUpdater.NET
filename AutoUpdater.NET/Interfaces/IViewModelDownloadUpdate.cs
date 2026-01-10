// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelDownloadUpdate
{
    double ProgressPercentage { get; set; }
}