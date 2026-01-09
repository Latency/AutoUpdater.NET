// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.Commands.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate
{
    [RelayCommand]
    public static void Skip(object? parameter) => (parameter as Window_Update)?.Close();


    [RelayCommand]
    public void RemindLater(object? parameter)
    {
        (parameter as Window_Update)?.Close();

        _windowService.ShowWindow<Window_RemindLater, ViewModelRemindLater>(Owner, ServiceProvider.GetRequiredService<ViewModelRemindLater>());
    }


    [RelayCommand]
    public void Update(object? parameter)
    {
        (parameter as Window_Update)?.Close();

        _windowService.ShowWindow<Window_DownloadUpdate, ViewModelDownloadUpdate>(Owner, ServiceProvider.GetRequiredService<ViewModelDownloadUpdate>());
    }
}