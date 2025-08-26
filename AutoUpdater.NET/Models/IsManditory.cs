// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IsManditory.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Attributes;
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
    [JsonComment("Normal         - Ignores 'Remind Later' and 'Skip' values set previously and will hide both buttons.\n" +
                 "Forced         - Same as Normal mode, minus showing the close box.\n" +
                 "ForcedDownload - Will start downloading and applying update without showing standard update dialog in addition to Forced mode behaviour.")]
    public Mode UpdateMode { get; set; }
}