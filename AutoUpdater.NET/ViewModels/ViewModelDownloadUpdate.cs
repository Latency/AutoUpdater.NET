// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

public sealed partial class ViewModelDownloadUpdate : ViewModelRestricted, IViewModelDownloadUpdate
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelDownloadUpdate(IServiceProvider serviceProvider, Window_DownloadUpdate window, IViewModelConfig vmConfig) : base(serviceProvider, window, vmConfig)
    {
    }


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    [ObservableProperty]
    public partial double ProgressPercentage { get; set; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties
}