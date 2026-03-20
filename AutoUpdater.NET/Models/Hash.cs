// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Hash.cs
// Author:   Latency McLaughlin
// Date:     02/19/2026
// ****************************************************************************

using System.Security.Authentication;
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Hash class for checksum.
/// </summary>
public abstract class Hash
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    protected Hash()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="obj"></param>
    protected Hash(Hash? obj) : this()
    {
        if (obj is null)
            return;

        HashingAlgorithm = obj.HashingAlgorithm;
        HashValue        = obj.HashValue;
    }


    #pragma warning disable SYSLIB0058
    public HashAlgorithmType HashingAlgorithm { get; init; }
    #pragma warning restore SYSLIB0058


    /// <summary>
    ///     Hash of the file.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? HashValue { get; set; }
}