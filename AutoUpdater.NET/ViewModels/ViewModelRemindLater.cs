// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelRemindLater.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

// ReSharper disable InconsistentNaming

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoUpdaterDotNET.Commands;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Views;

namespace AutoUpdaterDotNET.ViewModels;

public sealed class ViewModelRemindLater : DependencyObject, IViewModelRemindLater
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public ViewModelRemindLater() => CommandButtonOk = new RelayCommand(((IViewModelRemindLater)this).ButtonOk_Click, AllowReminder);


    // ReSharper disable once AsyncVoidMethod
    void IViewModelRemindLater.ButtonOk_Click(object? sender)
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
    private static bool AllowReminder(object? _) => true;

    public ICommand CommandButtonOk { get; set; }


    public RemindLaterFormat RemindLaterFormat { get; private set; }

    public int RemindLaterAt { get; private set; }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #endregion Properties
}