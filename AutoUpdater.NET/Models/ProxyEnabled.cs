// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ProxyEnabled.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************

using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record ProxyEnabled : UsernamePassword
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public ProxyEnabled()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="obj"></param>
    public ProxyEnabled(ProxyEnabled? obj) : base(obj)
    {
        if (obj is null)
            return;

        Uri = obj.Uri;
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Uri { get; set; }

    public override string ToString() => $"Uri: {Uri}, UserName: {UserName}, Password: {Password}";
}