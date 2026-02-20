// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelDownloadUpdate.Commands.cs
// Author:   Latency McLaughlin
// Date:     02/10/2026
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using CommunityToolkit.Mvvm.Input;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelDownloadUpdate
{
    [RelayCommand]
    private void Close()
    {
        var dl = _singletonDownload.Value;
        if (dl.Config is not { IsMandatory: true, UpdateMode: Mode.ForcedDownload })
            Download.HttpWebClient.CancelPendingRequests();

        dl.CTS?.Cancel();
    }
}