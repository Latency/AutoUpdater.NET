// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IsManditory.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Enums;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Mandatory class to fetch the XML values related to Mandatory field.
/// </summary>
public record IsManditory
{
    /// <summary>
    ///     Mode that should be used for this update.
    /// </summary>
    public Mode UpdateMode { get; set; }
}