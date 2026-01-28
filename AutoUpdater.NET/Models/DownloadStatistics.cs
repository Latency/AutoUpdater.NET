// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     DownloadStatistics.cs
// Author:   Latency McLaughlin
// Date:     01/27/2026
// ****************************************************************************

namespace AutoUpdaterDotNET.Models;

public record DownloadStatistics
{
    public long BytesReceived       { get; set; }
    public long TotalBytesToReceive { get; set; }
    public byte   ProgressPercentage  { get; set; }
}