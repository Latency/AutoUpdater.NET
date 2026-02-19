// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     UpdateInfoEventArgs.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Windows;

namespace AutoUpdaterDotNET.Models;

/// <summary>
///     Object of this class gives you all the details about the update useful in handling the update logic yourself.
/// </summary>
public class UpdateInfoEventArgs
{
    /// <summary>
    ///     BaseUri
    /// </summary>
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
    ///     Returns version of the application currently installed on the user's PC.
    /// </summary>
    public Version2? InstalledVersion { get; set; }

    /// <summary>
    ///     Command line arguments used by Installer.
    /// </summary>
    public string? InstallerArgs { get; set; }

    /// <summary>
    ///     Checksum of the update file.
    /// </summary>
    public CheckSum? CheckSum { get; set; }

    // ReSharper disable once InconsistentNaming
    private static string? GetURL(Uri? baseUri, string? url)
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