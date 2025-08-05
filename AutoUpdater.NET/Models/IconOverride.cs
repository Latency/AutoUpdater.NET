// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IconOverride.cs
// Author:   Latency McLaughlin
// Date:     08/04/2025
// ****************************************************************************

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Mandatory class to fetch the XML values related to Mandatory field.
/// </summary>
public record IconOverride
{
    /// <summary>
    ///     File path location for the image to override.
    /// </summary>
    public string? Uri { get; set; }
}