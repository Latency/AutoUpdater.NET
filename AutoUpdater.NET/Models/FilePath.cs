// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     FilePath.cs
// Author:   Latency McLaughlin
// Date:     08/04/2025
// ****************************************************************************

using AutoUpdaterDotNET.Attributes;
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record FilePath
{
    /// <summary>
    ///     File path location.
    /// </summary>
    [JsonComment("File path location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Path { get; set; }
}