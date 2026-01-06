// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     WindowService.cs
// Author:   Latency McLaughlin
// Date:     01/06/2026
// ****************************************************************************

using System.Windows;
using AutoUpdaterDotNET.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AutoUpdaterDotNET.Models;

public class WindowService(IServiceProvider serviceProvider) : IWindowService
{
    public void ShowWindow<TWindow, TViewModel>(TViewModel viewModel)
        where TWindow : Window
    {
        if (viewModel is null)
            throw new NullReferenceException();

        var window = serviceProvider.GetRequiredService<TWindow>();
        window.DataContext = viewModel;
        window.Show();
    }
}