// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelDownloadUpdate.Commands.cs
// Author:   Latency McLaughlin
// Date:     02/10/2026
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelDownloadUpdate
{
    [RelayCommand]
    private void Close()
    {
        var vm = Dependencies.ServiceProvider.GetRequiredService<IViewModelConfig>();
        if (vm.Config is not { IsMandatory: true, UpdateMode: Mode.ForcedDownload })
            ViewModelConfig.HttpWebClient.CancelPendingRequests();

        CTS?.Cancel();
    }
}