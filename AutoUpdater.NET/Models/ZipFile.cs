// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ZipFile.cs
// Author:   Latency McLaughlin
// Date:     08/04/2025
// ****************************************************************************

using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Mandatory class to fetch the XML values related to Mandatory field.
/// </summary>
public record ZipFile
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool      ClearAppDirectory             { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FilePath? ExecutablePathOverride        { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FilePath? ZipExtractionPathOverride     { get; set; }
}