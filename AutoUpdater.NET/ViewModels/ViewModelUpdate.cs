// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelUpdate : ObservableObject
{
    public ViewModelMainConfig Config { get; }


    /// <summary>
    /// Constructor
    /// </summary>
    public ViewModelUpdate(IServiceProvider provider)
    {
        Config = provider.GetRequiredService<ViewModelMain>();

        ToggleControlBox += OnToggleControlBox;

        if (Config is { IsManditory: true, UpdateMode: Mode.Forced })
        {
            var win = provider.GetRequiredService<Window_Update>();
            win.ControlBox = false;
        }
    }

    private void OnToggleControlBox(bool? obj)
    {
    }
}