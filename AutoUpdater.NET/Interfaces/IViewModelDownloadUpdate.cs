// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelDownloadUpdate
{
    double ProgressPercentage { get; set; }
}