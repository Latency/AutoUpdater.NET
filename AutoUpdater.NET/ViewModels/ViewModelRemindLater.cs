// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelRemindLater.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Windows;
using System.Windows.Controls;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.Input;

namespace AutoUpdaterDotNET.ViewModels;

public sealed partial class ViewModelRemindLater : DependencyObject
{
    [RelayCommand]
    public void RemindLater(object? sender)
    {
        var (vm, btnOk, args) = (sender as Tuple<Window_RemindLater, Button, RoutedEventArgs>)!;

        if (vm.RadioButtonYes is { IsChecked: true })
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

    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public RemindLaterFormat RemindLaterFormat { get; private set; }

    public int RemindLaterAt { get; private set; }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties
}