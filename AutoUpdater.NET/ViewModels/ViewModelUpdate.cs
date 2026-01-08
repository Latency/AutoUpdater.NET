// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate : ViewModelRestricted
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWindowService   _windowService;


    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelUpdate(IServiceProvider provider, IWindowService windowService, ViewModelConfig vmMain) : base(vmMain)
    {
        _serviceProvider = provider;
        _windowService   = windowService;
    }
}