// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     WindowServiceHostBuilder.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.ViewModels;
using AutoUpdaterDotNET.Views;
using Microsoft.Extensions.DependencyInjection;
using WindowService.Extensions;

namespace AutoUpdaterDotNET.Extensions;

// ReSharper disable once UnusedMember.Global
public static class WindowServiceHostBuilder
{
    /// <summary>
    /// Registers window service compoments to the the host builder service collection.
    /// </summary>
    /// <remarks>This extension method registers all necessary view models and windows required for the
    /// application's base window service feature.
    /// </remarks>
    public static void AutoUpdateWindowService(this IServiceCollection serviceCollection)
    {
        serviceCollection.WindowService();

        // Register ViewModels
        serviceCollection.AddSingleton<IViewModelConfig        , ViewModelConfig>();
        serviceCollection.AddSingleton<IViewModelRemindLater   , ViewModelRemindLater>();
        serviceCollection.AddSingleton<IViewModelUpdate        , ViewModelUpdate>();
        serviceCollection.AddSingleton<IViewModelDownloadUpdate, ViewModelDownloadUpdate>();

        // Register Views
        serviceCollection.AddSingleton<Window_Config>();
        serviceCollection.AddSingleton<Window_DownloadUpdate>();
        serviceCollection.AddSingleton<Window_RemindLater>();
        serviceCollection.AddSingleton<Window_Update>();
    }
}