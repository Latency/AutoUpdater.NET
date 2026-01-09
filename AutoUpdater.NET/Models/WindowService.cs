// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     WindowService.cs
// Author:   Latency McLaughlin
// Date:     01/06/2026
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AutoUpdaterDotNET.Controls;

namespace AutoUpdaterDotNET.Models;

public class WindowService(IServiceProvider serviceProvider) : IWindowService
{
    public void ShowWindow<TWindow, TViewModel>(IFrameworkInputElement? owner, TViewModel viewModel)
        where TWindow    : Window
        where TViewModel : ObservableObject
    {
        var window = serviceProvider.GetRequiredService<TWindow>();
        window.DataContext = viewModel;

        if (owner is not null)
            window.Owner = (owner as Window)!;

        if (window is Window_Restricted win && viewModel is ViewModelConfig vmc)
            win.Config = vmc;

        window.Show();
    }
}