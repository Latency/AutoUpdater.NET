// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     BaseServiceDependencies.cs
// Author:   Latency McLaughlin
// Date:     02/06/2026
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.ViewModels;

public class BaseServiceDependencies
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public IServiceProvider ServiceProvider;
    public IWindowService   WindowService;
    public IViewModelConfig Config;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    /// <summary>
    /// Constructor
    /// </summary>
    public BaseServiceDependencies(IServiceProvider serviceProvider, IWindowService windowService, IViewModelConfig vmConfig)
    {
        WindowService   = windowService;
        ServiceProvider = serviceProvider;
        Config          = vmConfig;
    }
}