// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     WindowService.cs
// Author:   Latency McLaughlin
// Date:     01/06/2026
// ****************************************************************************

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AutoUpdaterDotNET.ViewModels;

namespace AutoUpdaterDotNET.Models;

public partial class WindowService(IServiceProvider serviceProvider) : IWindowService
{
    private Window _window = null!;


    public TWindow InitializeWindow<TWindow, TViewModel>(IFrameworkInputElement? owner, TViewModel viewModel)
        where TWindow : Window
        where TViewModel : ObservableObject
    {
        _window = serviceProvider.GetRequiredService<TWindow>();
        _window.DataContext = viewModel;

        if (owner is not null)
            _window.Owner = (owner as Window)!;

        // ReSharper disable once InvertIf
        if (_window is Window_Restricted win)
        {
            win.Config ??= serviceProvider.GetRequiredService<IViewModelConfig>();
            if (viewModel is ViewModelRestricted vmc)
                vmc.Config ??= win.Config;

            win.Owner2 = win.Config.DoNotBindOwnerWindow ? null! : win.Owner!;
            win.Owner  = (win.Owner2 as Window)!;
        }

        return (_window as TWindow)!;
    }
}