// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelRemindLater.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using WindowService.ViewModels;
using kvp = System.Collections.Generic.KeyValuePair<ushort, AutoUpdaterDotNET.Enums.RemindLaterFormat>;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelRemindLater : ViewModelRestricted, IViewModelRemindLater
{
    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    [ObservableProperty]
    public partial ObservableCollection<kvp> ComboBoxItems { get; set; } =
    [
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


    [ObservableProperty]
    public partial IEnumerable<RemindLaterFormat> RemindLaterFormatEnumValues { get; set; } = Enum.GetValues<RemindLaterFormat>().Skip(1);


    public IConfig Config { get; init; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    /// <summary>
    ///     Default Constructor
    /// </summary>
    /// <param name="dependencies"></param>
    /// <param name="vm"></param>
    public ViewModelRemindLater(BaseServiceDependencies dependencies, IViewModelConfig vm) : base(dependencies)
    {
        Config = vm.Config;
    }
}