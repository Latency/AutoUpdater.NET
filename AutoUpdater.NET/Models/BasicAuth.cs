// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     BasicAuth.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************

using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record BasicAuth : UsernamePassword
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public BasicAuth()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="obj"></param>
    public BasicAuth(BasicAuth? obj) : base(obj)
    { }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ChangeLog{ get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Download { get; set; }

    public override string ToString() => $"ChangeLog: {ChangeLog}, Download: {Download}";
}