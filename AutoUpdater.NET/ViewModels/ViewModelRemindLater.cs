// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModel
// .cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using kvp = System.Collections.Generic.KeyValuePair<ushort, AutoUpdaterDotNET.Enums.RemindLaterFormat>;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelRemindLater : ViewModelRestricted, IViewModelRemindLater
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelRemindLater(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    [ObservableProperty]
    public partial ObservableCollection<kvp> ComboBoxItems { get; set; } = [
        new(05, RemindLaterFormat.Minutes),
        new(10, RemindLaterFormat.Minutes),
        new(15, RemindLaterFormat.Minutes),
        new(30, RemindLaterFormat.Minutes),
        new(01, RemindLaterFormat.Hours),
        new(06, RemindLaterFormat.Hours),
        new(12, RemindLaterFormat.Hours),
        new(01, RemindLaterFormat.Days),
        new(02, RemindLaterFormat.Days),
        new(04, RemindLaterFormat.Days),
        new(01, RemindLaterFormat.Weeks),
        new(02, RemindLaterFormat.Weeks)
    ];


    [ObservableProperty]
    public partial kvp ComboBoxSelectedItem { get; set; }
    partial void OnComboBoxSelectedItemChanged(kvp value)
    {
        Config.RemindLaterAt       = value.Key;
        Config.RemindLaterTimeSpan = value.Value;
    }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties
}