// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IWindowService.cs
// Author:   Latency McLaughlin
// Date:     01/07/2026
// ****************************************************************************

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.Interfaces;

public interface IWindowService
{
    void ShowWindow<TWindow, TViewModel>(IFrameworkInputElement? owner, TViewModel viewModel)
        where TWindow    : Window
        where TViewModel : ObservableObject;
}