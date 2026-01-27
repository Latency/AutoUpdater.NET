// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Mandatory.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Enums;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Mandatory class to fetch the XML values related to Mandatory field.
/// </summary>
public record Mandatory
{
    /// <summary>
    ///     Value of the Mandatory field.
    /// </summary>
    public bool Value { get; set; }

    /// <summary>
    ///     If this is set and 'Value' property is set to true then it will trigger the mandatory update only when current
    ///     installed version is less than value of this property.
    /// </summary>
    public string? MinimumVersion { get; set; }

    /// <summary>
    ///     Mode that should be used for this update.
    /// </summary>
    public Mode UpdateMode { get; set; }
}