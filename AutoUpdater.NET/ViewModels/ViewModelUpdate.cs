// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate : ObservableObject
{
    private IServiceProvider    _serviceProvider;

    public  ViewModelMainConfig Config { get; }


    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelUpdate(IServiceProvider provider)
    {
        _serviceProvider = provider;
        Config           = provider.GetRequiredService<ViewModelMain>();
    }
}