// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     FilePath.cs
// Author:   Latency McLaughlin
// Date:     08/04/2025
// ****************************************************************************

using AutoUpdaterDotNET.Attributes;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.Models;

public partial class FilePath : ObservableObject
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public FilePath()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="obj"></param>
    public FilePath(FilePath? obj)
    {
        if (obj is null)
            return;

        Path = obj.Path;
    }


    /// <summary>
    ///     File path location.
    /// </summary>
    [JsonComment("File path location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial string? Path { get; set; }
}