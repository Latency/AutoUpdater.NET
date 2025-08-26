// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     UsernamePassword.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************

using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record UsernamePassword
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? UserName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Password { get; set; }

    public override string ToString() => $"UserName: {UserName}, Password: {Password}";
}