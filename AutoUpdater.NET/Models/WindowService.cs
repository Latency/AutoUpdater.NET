// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     WindowService.cs
// Author:   Latency McLaughlin
// Date:     01/06/2026
// ****************************************************************************

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AutoUpdaterDotNET.Models;

public partial class WindowService : IWindowService
{
    private          Window           _window = null!;
    private readonly IServiceProvider _serviceProvider;
    private readonly IViewModelConfig _vmConfig;


    /// <summary>
    /// Default Constructor
    /// </summary>
    public WindowService(IServiceProvider serviceProvider, IViewModelConfig vmConfig)
    {
        _serviceProvider = serviceProvider;
        _vmConfig        = vmConfig;
    }


    public TWindow InitializeWindow<TWindow, TViewModel>(Window? owner, TViewModel viewModel)
        where TWindow : Window
    {
        _window = _serviceProvider.GetRequiredService<TWindow>();
        _window.DataContext = viewModel!;

        // ReSharper disable once InvertIf
        if (_window is Window_Restricted win)
        {
            win.Tag = owner!;
            if (viewModel is IViewModelConfig vm)
                win.SetBindings(vm);
            else
                win.SetBindings(_vmConfig);
        }
        else
        {
            _window.Owner = owner!;
        }

        return (_window as TWindow)!;
    }
}