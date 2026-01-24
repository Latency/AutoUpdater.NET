// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     CheckSum.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Checksum class to fetch the serialization values for checksum.
/// </summary>
public record CheckSum
{
    private readonly int _hashCode;


    /// <summary>
    ///     Constructor Overload +1
    /// </summary>
    public CheckSum(Stream stream)
    {
        HashData  = SHA256.HashData(stream);
        HashValue = Convert.ToHexString(HashData).ToLowerInvariant();

        var hash = new HashCode();
        hash.AddBytes(HashData);
        _hashCode = hash.ToHashCode();
    }


    /// <summary>
    ///     Constructor Overload +2
    /// </summary>
    public CheckSum(string plainText)
    {
        HashData  = SHA256.HashData(Encoding.UTF8.GetBytes(plainText));
        HashValue = Convert.ToHexString(HashData).ToLowerInvariant();

        var hash = new HashCode();
        hash.AddBytes(HashData);
        _hashCode = hash.ToHashCode();
    }


    /// <summary>
    ///     Hash of the file.
    /// </summary>
    public string HashValue { get; init; }


    /// <summary>
    ///     Hash algorithm that generated the hash.
    /// </summary>
    public byte[] HashData { get; init; }


    /// <summary>
    ///     Hash code based on the hash data generated.
    /// </summary>
    /// <returns><see cref="int"/></returns>
    public override int GetHashCode() => _hashCode;
}