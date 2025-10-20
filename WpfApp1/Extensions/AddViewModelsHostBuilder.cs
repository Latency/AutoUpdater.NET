// ****************************************************************************
// Project:  WpfApp1
// File:     AddViewModelsHostBuilder.cs
// Author:   Latency McLaughlin
// Date:     10/14/2025
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfApp1.View_Models;

namespace WpfApp1.Extensions;

internal static class AddViewModelsHostBuilderExtensions
{
    internal static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            // Register a hosted service
            services.AddSingleton<MainViewModel>();
        });
        return hostBuilder;
    }
}