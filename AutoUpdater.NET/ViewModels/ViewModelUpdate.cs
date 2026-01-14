// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Views;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate : ViewModelRestricted, IViewModelUpdate
{
    private readonly IViewModelDownloadUpdate _vmModelDownloadUpdate;
    private readonly IViewModelRemindLater    _vmRemindLater;
    private readonly IWindowService           _windowService;


    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelUpdate(IServiceProvider serviceProvider, IWindowService windowService, IViewModelDownloadUpdate vmModelDownloadUpdate, IViewModelRemindLater vmRemindLater, Window_Update window, IViewModelConfig vmConfig) : base(serviceProvider, window, vmConfig)
    {
        _windowService         = windowService;
        _vmRemindLater         = vmRemindLater;
        _vmModelDownloadUpdate = vmModelDownloadUpdate;
    }
}