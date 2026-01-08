// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     WindowService.cs
// Author:   Latency McLaughlin
// Date:     01/06/2026
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AutoUpdaterDotNET.ViewModels;

namespace AutoUpdaterDotNET.Models;

public class WindowService(IServiceProvider serviceProvider) : IWindowService
{
    public void ShowWindow<TWindow, TViewModel>(Window? owner, TViewModel viewModel)
        where TWindow    : Window
        where TViewModel : ObservableObject
    {
        if (viewModel is null)
            throw new NullReferenceException();

        var window = serviceProvider.GetRequiredService<TWindow>();
        window.DataContext = viewModel;

        if (owner is not null && viewModel is ViewModelRestricted vm)
            window.Owner = vm.Owner = owner;

        window.Show();
    }
}