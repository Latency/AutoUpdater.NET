// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     KeyValuePairItem.cs
// Author:   Latency McLaughlin
// Date:     01/05/2026
// ****************************************************************************

using AutoUpdaterDotNET.Enums;

namespace AutoUpdaterDotNET.Models;

public record KeyValuePairItem
{
    public ushort            Duration { get; set; }
    public RemindLaterFormat Value    { get; set; }
}