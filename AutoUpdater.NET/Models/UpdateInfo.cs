// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     UpdateInfo.cs
// Author:   Latency McLaughlin
// Date:     02/19/2025
// ****************************************************************************

using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public record UpdateInfo
{
    public required string Domain { get; init; }

    /// <summary>
    ///     BaseUri
    /// </summary>
    [JsonIgnore]
    public Uri BaseUri => new(Domain);

    /// <summary>
    ///     Download URL of the update file.
    /// </summary>
    [JsonIgnore]
    // ReSharper disable once InconsistentNaming
    public string? DownloadURL
    {
        get => GetURL(BaseUri, field);
        set;
    }

    /// <summary>
    ///     File of the application to be updated.
    /// </summary>
    public required string FileName { get; set; }

    /// <summary>
    ///     Version of the new assembly.
    /// </summary>
    /// <remarks>
    ///     Expected to be taken out of the assembly manifest.
    /// </remarks>
    public required Version Version { get; init;  }

    /// <summary>
    ///     Command line arguments used by Installer.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InstallerArgs { get; set; }

    /// <summary>
    ///     Hash information of the update file.
    /// </summary>
    public required Hash Hash { get; init; }

    // ReSharper disable once InconsistentNaming
    private static string? GetURL(Uri? baseUri, string? url)
    {
        // ReSharper disable once InvertIf
        if (baseUri is not null && !string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Relative))
        {
            var uri = new Uri(baseUri, url);
            if (uri.IsAbsoluteUri)
                url = uri.AbsoluteUri;
        }
        return url;
    }
}