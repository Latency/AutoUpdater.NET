// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     TimerEnabled.cs
// Author:   Latency McLaughlin
// Date:     08/03/2025
// ****************************************************************************

using AutoUpdaterDotNET.Attributes;
using AutoUpdaterDotNET.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.Models;

public partial class TimerEnabled : ObservableObject
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public TimerEnabled()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="te"></param>
    public TimerEnabled(TimerEnabled? te) : this()
    {
        if (te is null)
            return;

        Interval = te.Interval;
        TimeSpan = te.TimeSpan;
    }


    [ObservableProperty]
    [JsonComment("# of <TimeSpan>")]
    public partial ushort Interval { get; set; } = 1;

    [ObservableProperty]
    [JsonComment("0 - Seconds\n1 - Minutes\n2 - Hours\n3 - Days")]
    public partial RemindLaterFormat TimeSpan { get; set; } = RemindLaterFormat.Seconds;

    public override string ToString() => $"Interval: {Interval}, TimeSpan: {TimeSpan}";
}