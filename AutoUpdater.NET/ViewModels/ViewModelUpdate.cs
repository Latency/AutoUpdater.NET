// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate : ViewModelRestricted, IViewModelUpdate
{
    private readonly IWindowService _windowService;


    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelUpdate(IServiceProvider serviceProvider, IWindowService windowService) : base(serviceProvider)
    {
        _windowService = windowService;
    }
}