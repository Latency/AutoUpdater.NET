// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Version2.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record Version2
{
    /// <summary>
    ///     Overridden version of the assembly.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Version? Version { get; set; }

    public override string? ToString() => Version?.ToString();
}