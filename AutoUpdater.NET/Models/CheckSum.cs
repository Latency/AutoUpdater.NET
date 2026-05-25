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
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Checksum class to fetch the serialization values for checksum.
/// </summary>
public class CheckSum
{
    private int _hashCode;


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public CheckSum()
    {
    }


    public CheckSum(CheckSum? obj) : this()
    {
        if (obj is null)
            return;

        Algorithm        = obj.HashingAlgorithm.ToString();
        HashingAlgorithm = obj.HashingAlgorithm;
        HashValue        = obj.HashValue;
        HashData         = obj.HashData;
    }


    /// <summary>
    ///     Constructor Overload +1
    /// </summary>
    #pragma warning disable SYSLIB0058
    public CheckSum(Stream stream, HashAlgorithmType hat = HashAlgorithmType.Sha256)
    #pragma warning restore SYSLIB0058
    {
        Algorithm = hat.ToString();
        CalculateHash(stream);
        HashValue = Convert.ToHexString(HashData!).ToLowerInvariant();
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
    public CheckSum(byte[] bytes, HashAlgorithmType hat = HashAlgorithmType.Sha256)
    #pragma warning restore SYSLIB0058
    {
        Algorithm        = hat.ToString();
        CalculateHash(bytes);
        HashValue = Convert.ToHexString(HashData!).ToLowerInvariant();
    }


    private void CalculateHash(dynamic b)
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
    }


    /// <summary>
    ///     Hash algorithm that generated the hash.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? HashData
    {
        get;
        set
        {
            field = value;

            var hash = new HashCode();
            hash.AddBytes(value);
            _hashCode = hash.ToHashCode();
        }
    }


    /// <summary>
    /// Gets or sets the hash algorithm used to compute message digests.
    /// </summary>
    [JsonPropertyName("HashingAlgorithm")]
    public string? Algorithm
    {
        get;
        init
        {
            field = value ?? "None";
            HashingAlgorithm = field.ToUpper().Replace("-", string.Empty) switch
            {
                #pragma warning disable SYSLIB0058
                "NONE"   => HashAlgorithmType.None,
                "MD5"    => HashAlgorithmType.Md5,
                "SHA1"   => HashAlgorithmType.Sha1,
                "SHA256" => HashAlgorithmType.Sha256,
                "SHA384" => HashAlgorithmType.Sha384,
                "SHA512" => HashAlgorithmType.Sha512,
                #pragma warning restore SYSLIB0058
                _ => throw new IndexOutOfRangeException()
            };
        }
    }


    /// <summary>
    /// Gets or sets the hash algorithm used to compute message digests.
    /// </summary>
    /// <remarks>Some algorithms may be deprecated or unavailable on certain platforms; prefer modern
    /// algorithms such as SHA-256. The value is ignored during JSON serialization.</remarks>
    #pragma warning disable SYSLIB0058
    [JsonIgnore]
    public HashAlgorithmType HashingAlgorithm { get; set; }
    #pragma warning restore SYSLIB0058


    /// <summary>
    ///     Hash of the file.
    /// </summary>
    public string? HashValue
    {
        get;
        set
        {
            field = value;

            if (field is not null)
                CalculateHash(Encoding.UTF8.GetBytes(field));
        }
    }


    /// <summary>
    ///     Hash code based on the hash data generated.
    /// </summary>
    /// <returns><see cref="int"/></returns>
    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => _hashCode;
}