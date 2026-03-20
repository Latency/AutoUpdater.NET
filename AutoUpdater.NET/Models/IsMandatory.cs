// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IsMandatory.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Attributes;
using AutoUpdaterDotNET.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Mandatory class to fetch the XML values related to Mandatory field.
/// </summary>
public partial class IsMandatory : ObservableObject
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public IsMandatory()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="obj"></param>
    public IsMandatory(IsMandatory? obj) : this()
    {
        if (obj is null)
            return;

        UpdateMode = obj.UpdateMode;
    }


    /// <summary>
    ///     Mode that should be used for this update.
    /// </summary>
    [JsonComment("Normal         - Ignores 'Remind Later' and 'Skip' values set previously and will hide both buttons.\n" +
                 "Forced         - Same as Normal mode, minus showing the close box.\n" +
                 "ForcedDownload - Will start downloading and applying update without showing standard update dialog in addition to Forced mode behaviour.")]
    [ObservableProperty]
    public partial Mode UpdateMode { get; set; }
}