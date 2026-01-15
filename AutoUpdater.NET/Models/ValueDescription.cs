// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ValueDescription.cs
// Author:   Latency McLaughlin
// Date:     01/14/2026
// ****************************************************************************

namespace AutoUpdaterDotNET.Models;

public record ValueDescription
{
    public object? Value       { get; set; }
    public object? Description { get; set; }
}