// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     HttpClient.cs
// Author:   Latency McLaughlin
// Date:     02/03/2026
// ****************************************************************************

using AutoUpdaterDotNET.Properties;
using System.IO;
using System.Net;
using System.Net.Http;

namespace AutoUpdaterDotNET.Extensions;

public static class HttpClientExtensions
{
    public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<double>? progress = null, Action<long, long>? callback = null, CancellationToken cancellationToken = default)
    {
        // Get the http headers first to examine the content length
        using var response      = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var       contentLength = response.Content.Headers.ContentLength;

        // Try to parse the content disposition header if it exists.
        if (response.Content.Headers.ContentDisposition != null)
        {
            var fileName = string.IsNullOrEmpty(response.Content.Headers.ContentDisposition?.FileName)
                               ? Path.GetFileName(response.RequestMessage?.RequestUri?.LocalPath)
                               : response.Content.Headers.ContentDisposition.FileName;

            if (string.IsNullOrWhiteSpace(fileName))
                throw new WebException(Settings.Default?.UnableToDetermineFilenameMessage);
        }

        await using var download = await response.Content.ReadAsStreamAsync(cancellationToken);
        // Ignore progress reporting when no progress reporter was passed or when the content length is unknown
        if (progress == null || !contentLength.HasValue)
        {
            await download.CopyToAsync(destination, cancellationToken);
            return;
        }

        // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
        var relativeProgress = new Progress<long>(totalBytes =>
        {
            progress.Report((double)totalBytes / contentLength.Value);
            callback?.Invoke(totalBytes, contentLength.Value);
        });
        // Use extension method to report progress while downloading
        await download.CopyToAsync(destination, 81920, relativeProgress, cancellationToken);
        progress.Report(1);
    }
}