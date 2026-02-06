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

        Dependencies.WindowService.InitializeWindow<Window_RemindLater, IViewModelRemindLater>(Window).Show();
    }


    [RelayCommand]
    private async Task Update(object? parameter)
    {
        (parameter as Window_Update)?.Close();

        var dlWin = Dependencies.WindowService.InitializeWindow<Window_DownloadUpdate, IViewModelDownloadUpdate>(Window);
        if (Config.OpenDownloadPage)
            dlWin.Show();
        else
        {
            if (dlWin.DataContext is IViewModelDownloadUpdate vmModelDownloadUpdate)
                await vmModelDownloadUpdate.DownloadUpdate(); // Start automatic updates!
        }
    }
}