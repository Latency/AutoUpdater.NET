// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IWindowService.cs
// Author:   Latency McLaughlin
// Date:     01/06/2026
// ****************************************************************************

using System.Windows;

namespace AutoUpdaterDotNET.Interfaces;

public interface IWindowService
{
    void ShowWindow<TWindow, TViewModel>(TViewModel viewModel)
        where TWindow : Window;
}