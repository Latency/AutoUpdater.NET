// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     TimerEnabled.cs
// Author:   Latency McLaughlin
// Date:     08/03/2025
// ****************************************************************************

using AutoUpdaterDotNET.Attributes;
using AutoUpdaterDotNET.Enums;
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record TimerEnabled
{
    [JsonComment("# of <TimeSpan>")]
    public ushort Interval { get; set; } = 1;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonComment("0 - Seconds\n1 - Minutes\n2 - Hours\n3 - Days")]
    public RemindLaterFormat? TimeSpan { get; set; }

    public override string ToString() => $"Interval: {Interval}, TimeSpan: {TimeSpan}";
}