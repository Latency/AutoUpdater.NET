// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     CheckSum.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.IO;
using System.Security.Authentication;
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
        var b = Encoding.UTF8.GetBytes(plainText);
        #pragma warning disable CS0618 // Type or member is obsolete
        HashData = HashingAlgorithm switch
        #pragma warning restore CS0618 // Type or member is obsolete
        {
            #pragma warning disable SYSLIB0058
            HashAlgorithmType.None   => MD5.HashData(b),
            HashAlgorithmType.Md5    => MD5.HashData(b),
            HashAlgorithmType.Sha1   => SHA1.HashData(b),
            HashAlgorithmType.Sha256 => SHA256.HashData(b),
            HashAlgorithmType.Sha384 => SHA384.HashData(b),
            HashAlgorithmType.Sha512 => SHA3_512.HashData(b),
            _                        => throw new ArgumentOutOfRangeException()
            #pragma warning restore SYSLIB0058
        };
        HashValue = Convert.ToHexString(HashData).ToLowerInvariant();

        var hash = new HashCode();
        hash.AddBytes(HashData);
        _hashCode = hash.ToHashCode();
    }


    #pragma warning disable SYSLIB0058
    public HashAlgorithmType HashingAlgorithm { get; set; }
    #pragma warning restore SYSLIB0058


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