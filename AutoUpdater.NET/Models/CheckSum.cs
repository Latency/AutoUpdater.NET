// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     CheckSum.cs
// Author:   Latency McLaughlin
// Date:     05/16/2025
// ****************************************************************************

using System.Xml.Serialization;

namespace AutoUpdaterDotNET;

/// <summary>
///     Checksum class to fetch the XML values for checksum.
/// </summary>
public class CheckSum
{
    /// <summary>
    ///     Hash of the file.
    /// </summary>
    [XmlText]
    public string? Value { get; set; }

    /// <summary>
    ///     Hash algorithm that generated the hash.
    /// </summary>
    [XmlAttribute("algorithm")]
    public string? HashingAlgorithm { get; set; }
}