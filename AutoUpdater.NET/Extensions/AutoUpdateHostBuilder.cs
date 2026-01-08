// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     AutoUpdateHostBuilder.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.ViewModels;
using AutoUpdaterDotNET.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoUpdaterDotNET.Extensions;

public static class AutoUpdateBuilderExtensions
{
    /// <summary>
    /// Configures the host builder to enable automatic update functionality by registering required services and views.
    /// </summary>
    /// <remarks>This extension method registers all necessary view models and windows required for the
    /// application's auto-update feature. Call this method during application startup to ensure update-related
    /// functionality is available throughout the application's lifetime.</remarks>
    /// <param name="hostBuilder">The host builder to configure with automatic update services. Cannot be null.</param>
    /// <returns>The same instance of <see cref="IHostBuilder"/> for chaining further configuration.</returns>
    public static IHostBuilder AutoUpdate(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddSingleton<IWindowService, WindowService>();

            // Register ViewModels
            services.AddSingleton<ViewModelConfig>();
            services.AddSingleton<ViewModelMainConfig>();
            services.AddSingleton<ViewModelDownloadUpdate>();
            services.AddSingleton<ViewModelRemindLater>();
            services.AddSingleton<ViewModelUpdate>();

            // Register Views
            services.AddSingleton<Window_Config>();
            services.AddSingleton<Window_AutoUpdater>();
            services.AddSingleton<Window_DownloadUpdate>();
            services.AddSingleton<Window_RemindLater>();
            services.AddSingleton<Window_Update>();
        });
        return hostBuilder;
    }
}