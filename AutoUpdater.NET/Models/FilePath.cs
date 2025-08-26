// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     FilePath.cs
// Author:   Latency McLaughlin
// Date:     08/04/2025
// ****************************************************************************

using AutoUpdaterDotNET.Attributes;

namespace AutoUpdaterDotNET.Models;

public record FilePath
{
    /// <summary>
    ///     File path location.
    /// </summary>
    [JsonComment("File path location")]
    public string? Path { get; set; }
}