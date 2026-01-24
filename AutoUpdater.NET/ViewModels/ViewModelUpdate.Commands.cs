// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.Commands.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.Input;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate
{
    [RelayCommand]
    private static void Skip(object? parameter) => (parameter as Window_Update)?.Close();


    [RelayCommand]
    private void RemindLater(object? parameter)
    {
        (parameter as Window_Update)?.Close();

        _windowService.InitializeWindow<Window_RemindLater, IViewModelRemindLater>(Window, _vmRemindLater).Show();
    }


    [RelayCommand]
    private void Update(object? parameter)
    {
        (parameter as Window_Update)?.Close();

        if (Config.OpenDownloadPage)
            _windowService.InitializeWindow<Window_DownloadUpdate, IViewModelDownloadUpdate>(Window, _vmModelDownloadUpdate).Show();

        // TODO
        // Start automatic updates!
    }
}