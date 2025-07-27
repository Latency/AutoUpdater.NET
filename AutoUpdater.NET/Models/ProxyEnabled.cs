// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ProxyEnabled.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************

using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record ProxyEnabled
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Uri { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? UserName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Password { get; set; }

    public override string? ToString() => $"Uri: {Uri}, UserName: {UserName}, Password: {Password}";
}