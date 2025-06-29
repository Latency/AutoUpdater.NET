// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     CheckSum.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Checksum class to fetch the XML values for checksum.
/// </summary>
public record CheckSum
{
    /// <summary>
    ///     Hash of the file.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    ///     Hash algorithm that generated the hash.
    /// </summary>
    public string? HashingAlgorithm { get; set; }
}