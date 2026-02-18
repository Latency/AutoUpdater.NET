// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Encodings.cs
// Author:   Latency McLaughlin
// Date:     02/13/2026
// ****************************************************************************
// ReSharper disable InconsistentNaming
namespace AutoUpdaterDotNET.Enums;

/// <summary>
///     Enum representing the effect of Mandatory flag.
/// </summary>
public enum Encodings
{
    Default,
    ASCII,
    BigEndianUnicode,
    Latin1,
    UTF32,
    UTF8,
    Unicode
}