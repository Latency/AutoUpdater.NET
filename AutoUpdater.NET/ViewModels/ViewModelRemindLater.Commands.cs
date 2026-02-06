// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.Commands.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.Input;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xceed.Wpf.Toolkit;
using kvp = System.Collections.Generic.KeyValuePair<ushort, AutoUpdaterDotNET.Enums.RemindLaterFormat>;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelRemindLater
{
    [RelayCommand]
    private void RemindLaterTimeSpanChanged()
    {
        RemindLater(Config.RemindLaterAt, Config.RemindLaterTimeSpan);

        if (Config.RemindLaterAt != ComboBoxSelectedItem.Key)
            Config.RemindLaterAt = ComboBoxSelectedItem.Key;
        if (Config.RemindLaterTimeSpan != ComboBoxSelectedItem.Value)
            Config.RemindLaterTimeSpan = ComboBoxSelectedItem.Value;
    }


    [RelayCommand]
    private void RemindLaterAtChanged(object? parameter)
    {
        var    timeVal        = (ushort) ((IntegerUpDown)parameter!).Value!;
        var    remindTimeSpan = ComboBoxSelectedItem.Value;

        RemindLater(timeVal, remindTimeSpan);

        if (Config.RemindLaterAt != ComboBoxSelectedItem.Key)
            Config.RemindLaterAt = ComboBoxSelectedItem.Key;
        if (Config.RemindLaterTimeSpan != ComboBoxSelectedItem.Value)
            Config.RemindLaterTimeSpan = ComboBoxSelectedItem.Value;
    }

    private void RemindLater(ushort timeVal, RemindLaterFormat remindTimeSpan)
    {
        ushort remindAt;

        switch (Config.RemindLaterTimeSpan)
        {
            case RemindLaterFormat.Seconds:
                switch (timeVal)
                {
                    case <= 5 * 60:
                    case (10 * 60) - 1:
                        remindAt = 5;
                        remindTimeSpan = RemindLaterFormat.Minutes;
                        break;
                    case <= 10 * 60:
                    case (15 * 60) - 1:
                        remindAt = 10;
                        remindTimeSpan = RemindLaterFormat.Minutes;
                        break;
                    case <= 15 * 60:
                    case (30 * 60) - 1:
                        remindAt = 15;
                        remindTimeSpan = RemindLaterFormat.Minutes;
                        break;
                    case <= 30 * 60:
                        remindAt = 30;
                        remindTimeSpan = RemindLaterFormat.Minutes;
                        break;
                    default:
                        remindAt       = 1;
                        remindTimeSpan = RemindLaterFormat.Hours;
                        break;
                }
                break;
            case RemindLaterFormat.Minutes:
                switch (timeVal)
                {
                    case <= 5:
                    case 9:
                        remindAt = 5;
                        break;
                    case <= 10:
                    case 14:
                        remindAt = 10;
                        break;
                    case <= 15:
                    case 29:
                        remindAt = 15;
                        break;
                    case <= 30:
                        remindAt = 30;
                        break;
                    default:
                        remindAt       = 1;
                        remindTimeSpan = RemindLaterFormat.Hours;
                        break;
                }
                break;
            case RemindLaterFormat.Hours:
                switch (timeVal)
                {
                    case <= 1:
                    case 5:
                        remindAt = 1;
                        break;
                    case <= 6:
                    case 11:
                        remindAt = 6;
                        break;
                    case <= 12:
                        remindAt = 12;
                        break;
                    default:
                        remindAt       = 1;
                        remindTimeSpan = RemindLaterFormat.Days;
                        break;
                }
                break;
            case RemindLaterFormat.Days:
                switch (timeVal)
                {
                    case <= 2:
                        remindAt = timeVal;
                        break;
                    case 3:
                        remindAt = (ushort)(timeVal < ComboBoxSelectedItem.Key ? 2 : 4);
                        break;
                    case <= 4:
                        remindAt = 4;
                        break;
                    default:
                        remindAt       = 1;
                        remindTimeSpan = RemindLaterFormat.Weeks;
                        break;
                }
                break;
            case RemindLaterFormat.Weeks:
                remindAt = timeVal switch
                {
                    <= 2 => timeVal,
                    _    => 2
                };
                break;
            default:
                throw new ArgumentOutOfRangeException(MethodBase.GetCurrentMethod()?.Name);
        }

        ComboBoxSelectedItem = new kvp(remindAt, remindTimeSpan);
    }


    [RelayCommand]
    private void RemindLater(object? parameter)
    {
        (parameter as Window_RemindLater)?.Close();

        var vmModelDownloadUpdate = Dependencies.ServiceProvider.GetRequiredService<IViewModelDownloadUpdate>();

        if (Config.OpenDownloadPage)
            Dependencies.WindowService.InitializeWindow<Window_DownloadUpdate, IViewModelDownloadUpdate>(((IViewModelRestricted)this).Owner).Show();

        // TODO
        // Start automatic updates!
    }
}