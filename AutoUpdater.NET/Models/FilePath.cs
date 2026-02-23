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
    ///     File path location.
    /// </summary>
    [JsonComment("File path location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial string? Path { get; set; }
}