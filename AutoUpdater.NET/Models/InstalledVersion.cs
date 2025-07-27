// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     InstalledVersion.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record InstalledVersion
{
    /// <summary>
    ///     Overridden version of the assembly.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Version? Version { get; set; }

    public override string? ToString() => Version?.ToString();
}