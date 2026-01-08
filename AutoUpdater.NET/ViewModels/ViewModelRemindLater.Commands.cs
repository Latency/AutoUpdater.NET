// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.Commands.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.Input;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelRemindLater
{
    [RelayCommand]
    public void RemindLater(object? parameter)
    {
        (parameter as Window_RemindLater)?.Close();

        // TODO
        // Start automatic updates!
    }
}