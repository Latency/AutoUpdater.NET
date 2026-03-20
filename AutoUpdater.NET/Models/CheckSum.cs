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
public class CheckSum : Hash
{
    private readonly int _hashCode;


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public CheckSum() : this(null)
    { }


    public CheckSum(CheckSum? obj) : base(obj)
    {
        if (obj is null)
            return;

        HashData = obj.HashData;
    }


    /// <summary>
    ///     Constructor Overload +1
    /// </summary>
    #pragma warning disable SYSLIB0058
    public CheckSum(Stream stream, HashAlgorithmType hat = HashAlgorithmType.Sha256) : this()
    #pragma warning restore SYSLIB0058
    {
        HashingAlgorithm = hat;
        _hashCode        = CalculateHash(stream);
    }


    /// <summary>
    ///     Constructor Overload +2
    /// </summary>
    #pragma warning disable SYSLIB0058
    public CheckSum(string plainText, HashAlgorithmType hat = HashAlgorithmType.Sha256) : this(Encoding.UTF8.GetBytes(plainText), hat)
    #pragma warning restore SYSLIB0058
    {
    }


    /// <summary>
    ///     Constructor Overload +3
    /// </summary>
    #pragma warning disable SYSLIB0058
    public CheckSum(byte[] bytes, HashAlgorithmType hat = HashAlgorithmType.Sha256) : this()
    #pragma warning restore SYSLIB0058
    {
        HashingAlgorithm = hat;
        _hashCode = CalculateHash(bytes);
    }


    private int CalculateHash(dynamic b)
    {
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
        return hash.ToHashCode();
    }


    /// <summary>
    ///     Hash algorithm that generated the hash.
    /// </summary>
    public byte[] HashData { get; private set; } = [];


    /// <summary>
    ///     Hash code based on the hash data generated.
    /// </summary>
    /// <returns><see cref="int"/></returns>
    public override int GetHashCode() => _hashCode;
}