// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ParseUpdateInfoEventArgs.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     An object of this class contains the AppCast file received from server.
/// </summary>
public class ParseUpdateInfoEventArgs(string? remoteData) : UpdateInfoEventArgs
{
    /// <summary>
    ///     Remote data received from the AppCast file.
    /// </summary>
    public string? RemoteData { get; } = remoteData;
}