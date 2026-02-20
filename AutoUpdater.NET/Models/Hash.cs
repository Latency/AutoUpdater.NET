// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Hash.cs
// Author:   Latency McLaughlin
// Date:     02/19/2026
// ****************************************************************************

using System.Security.Authentication;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Hash class for checksum.
/// </summary>
public record Hash
{
    #pragma warning disable SYSLIB0058
    public HashAlgorithmType HashingAlgorithm { get; init; }
    #pragma warning restore SYSLIB0058


    /// <summary>
    ///     Hash of the file.
    /// </summary>
    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public string HashValue { get; set; }
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}