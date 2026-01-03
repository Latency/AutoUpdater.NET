// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.Commands.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.Input;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelRemindLater
{
    [RelayCommand]
    public void RemindLater(object? sender)
    {
        var vm = sender as Window_RemindLater;

        if (OptionYesSelected)
        {
            if (vm.ComboBoxRemindLater == null)
                throw new NullReferenceException();

            switch (vm.ComboBoxRemindLater.SelectedIndex)
            {
                case 0:
                    RemindLaterFormat = RemindLaterFormat.Minutes;
                    RemindLaterAt     = 30;
                    break;
                case 1:
                    RemindLaterFormat = RemindLaterFormat.Hours;
                    RemindLaterAt     = 12;
                    break;
                case 2:
                    RemindLaterFormat = RemindLaterFormat.Days;
                    RemindLaterAt     = 1;
                    break;
                case 3:
                    RemindLaterFormat = RemindLaterFormat.Days;
                    RemindLaterAt     = 2;
                    break;
                case 4:
                    RemindLaterFormat = RemindLaterFormat.Days;
                    RemindLaterAt     = 4;
                    break;
                case 5:
                    RemindLaterFormat = RemindLaterFormat.Days;
                    RemindLaterAt     = 8;
                    break;
                case 6:
                    RemindLaterFormat = RemindLaterFormat.Days;
                    RemindLaterAt     = 10;
                    break;
            }
        }
        else // No... Perform update!
        {
            ;
        }

        vm.Hide();
    }
}