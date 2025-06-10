namespace AutoUpdaterDotNET.Models;

/// <summary>
///     An object of this class contains the AppCast file received from server.
/// </summary>
public class ParseUpdateInfoEventArgs(AutoUpdater owner, string remoteData) : EventArgs
{
    /// <summary>
    ///     Remote data received from the AppCast file.
    /// </summary>
    public string RemoteData { get; init; } = remoteData;

    /// <summary>
    ///     Set this object with values received from the AppCast file.
    /// </summary>
    public UpdateInfoEventArgs UpdateInfo { get; set; } = new()
        { Owner = owner };
}