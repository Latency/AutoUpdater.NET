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
    public void Skip(object? parameter)
    {
        var win = parameter as Window_Update;
        win?.Close();
    }


    [RelayCommand]
    public void RemindLater(object? parameter)
    {
        var win = parameter as Window_Update;
        win?.Hide();

        var remindWin = _serviceProvider.GetRequiredService<Window_RemindLater>();
        remindWin.ShowDialog();
    }


    [RelayCommand]
    public void Update(object? parameter)
    {
        var win = parameter as Window_Update;
        win?.Close();
    }
}