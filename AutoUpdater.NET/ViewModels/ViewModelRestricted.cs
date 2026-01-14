// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelRestricted.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

public abstract class ViewModelRestricted : ObservableObject, IViewModelRestricted
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected                  IServiceProvider  ServiceProvider;
    private protected readonly Window_Restricted Window;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public IViewModelMainConfig Config { get; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    /// <summary>
    /// Constructor
    /// </summary>
    protected ViewModelRestricted(IServiceProvider serviceProvider, Window_Restricted window, IViewModelConfig vmConfig)
    {
        Window          = window;
        ServiceProvider = serviceProvider;
        Config          = vmConfig;
    }
}