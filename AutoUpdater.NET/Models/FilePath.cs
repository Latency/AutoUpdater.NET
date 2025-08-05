// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     FilePath.cs
// Author:   Latency McLaughlin
// Date:     08/04/2025
// ****************************************************************************

namespace AutoUpdaterDotNET.Models;

public record FilePath
{
    /// <summary>
    ///     File path location.
    /// </summary>
    public string? Path { get; set; }
}