// ****************************************************************************
// Project:  BHI
// File:     IViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     04/18/2025
// ****************************************************************************

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelDownloadUpdate
{
    double ProgressPercentage { get; set; }
}