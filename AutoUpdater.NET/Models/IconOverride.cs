// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IconOverride.cs
// Author:   Latency McLaughlin
// Date:     08/04/2025
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Mandatory class to fetch the XML values related to Mandatory field.
/// </summary>
public partial class IconOverride : ObservableObject
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public IconOverride()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="obj"></param>
    public IconOverride(IconOverride? obj)
    {
        if (obj is null)
            return;

        Uri = obj.Uri;
    }


    /// <summary>
    ///     File path location for the image to override.
    /// </summary>
    [ObservableProperty]
    public partial Uri? Uri { get; set; }

    public override string? ToString() => Uri?.ToString();
}