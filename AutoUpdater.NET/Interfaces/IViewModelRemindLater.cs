// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelRemindLater.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Collections.ObjectModel;
using AutoUpdaterDotNET.Enums;
using CommunityToolkit.Mvvm.Input;
using WindowService.Interfaces;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelRemindLater : IViewModelRestricted
{
    ObservableCollection<KeyValuePair<ushort, RemindLaterFormat>> ComboBoxItems { get; set; }

    KeyValuePair<ushort, RemindLaterFormat> ComboBoxSelectedItem { get; set; }

    IRelayCommand<object?> RemindLaterCommand { get; }
}