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
using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using kvp = System.Collections.Generic.KeyValuePair<ushort, AutoUpdaterDotNET.Enums.RemindLaterFormat>;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelRemindLater : ViewModelRestricted, IViewModelRemindLater
{
    private readonly IViewModelDownloadUpdate _vmModelDownloadUpdate;
    private readonly IWindowService           _windowService;


    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelRemindLater(IServiceProvider serviceProvider, IWindowService windowService, IViewModelDownloadUpdate vmModelDownloadUpdate, Window_RemindLater window, IViewModelConfig vmConfig) : base(serviceProvider, window, vmConfig)
    {

        _windowService         = windowService;
        _vmModelDownloadUpdate = vmModelDownloadUpdate;
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


    [ObservableProperty]
    public partial IEnumerable<RemindLaterFormat> RemindLaterFormatEnumValues { get; set; } = Enum.GetValues<RemindLaterFormat>().Skip(1);

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #endregion Properties
}