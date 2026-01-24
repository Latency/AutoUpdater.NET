// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     UpdateInfoEventArgs.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Text.Json.Serialization;
using System.Windows;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Object of this class gives you all the details about the update useful in handling the update logic yourself.
/// </summary>
public class UpdateInfoEventArgs : EventArgs
{
    /// <summary>
    ///     Resizes the update window.
    /// </summary>
    public Size? UpdateFormSize { get; set; }

    /// <summary>
    ///     If new update is available then returns true otherwise false.
    /// </summary>
    public bool IsUpdateAvailable { get; set; }

    /// <summary>
    ///     If there is an error while checking for update then this property won't be null.
    /// </summary>
    [JsonIgnore]
    public Exception? Error { get; set; }

    public Uri? BaseUri { get; set; }

    /// <summary>
    ///     Download URL of the update file.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public string? DownloadURL
    {
        get => GetURL(BaseUri, field);
        init;
    } = string.Empty;

    /// <summary>
    ///     URL of the webpage specifying changes in the new update.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public string? ChangelogURL
    {
        get => GetURL(BaseUri, field);
        set;
    } = string.Empty;

    /// <summary>
    ///     Returns newest version of the application available to download.
    /// </summary>
    public string? CurrentVersion { get; set; }

    /// <summary>
    ///     Returns version of the application currently installed on the user's PC.
    /// </summary>
    public InstalledVersion? InstalledVersion { get; set; }

    /// <summary>
    ///     Shows if the update is required or optional.
    /// </summary>
    public Mandatory Mandatory { get; set; } = new();

    /// <summary>
    ///     Executable path of the updated application relative to installation directory.
    /// </summary>
    public string? ExecutablePath { get; set; }

    /// <summary>
    ///     Command line arguments used by Installer.
    /// </summary>
    public string? InstallerArgs { get; set; }

    /// <summary>
    ///     Checksum of the update file.
    /// </summary>
    public CheckSum? CheckSum { get; set; }

    /// <summary>
    ///     Date/Time value for <see cref="UpdateInfoEventArgs" />
    /// </summary>
    [JsonIgnore]
    public DateTime TimeStamp { get; set; }

    // ReSharper disable once InconsistentNaming
    internal static string? GetURL(Uri? baseUri, string? url)
    {
        if (baseUri is not null && !string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Relative))
        {
            var uri = new Uri(baseUri, url);
            if (uri.IsAbsoluteUri)
                url = uri.AbsoluteUri;
        }

        return url;
    }
}