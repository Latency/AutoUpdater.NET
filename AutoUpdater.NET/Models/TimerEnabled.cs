// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     TimerEnabled.cs
// Author:   Latency McLaughlin
// Date:     08/03/2025
// ****************************************************************************

using AutoUpdaterDotNET.Enums;

namespace AutoUpdaterDotNET.Models;

public record TimerEnabled
{
    public ushort Interval { get; set; } = 1;

    public RemindLaterFormat TimeSpan { get; set; }

    public override string? ToString() => $"Interval: {Interval}, TimeSpan: {TimeSpan}";
}