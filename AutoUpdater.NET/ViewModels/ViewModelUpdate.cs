// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWindowService   _windowService;

    public  ViewModelMainConfig Config { get; }


    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelUpdate(IServiceProvider provider, IWindowService windowService, ViewModelMain vm)
    {
        _serviceProvider = provider;
        _windowService   = windowService;
        Config           = vm;
    }
}