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
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public UsernamePassword()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="obj"></param>
    public UsernamePassword(UsernamePassword? obj)
    {
        if (obj is null)
            return;

        UserName = obj.UserName;
        Password = obj.Password;
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? UserName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Password { get; set; }

    public override string ToString() => $"UserName: {UserName}, Password: {Password}";
}