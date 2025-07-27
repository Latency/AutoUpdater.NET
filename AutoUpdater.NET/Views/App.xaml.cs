// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     App.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AutoUpdaterDotNET.Views;

public partial class App
{
    private static IServiceProvider? _serviceProvider;


    protected override void OnStartup(StartupEventArgs? e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();
        var mainWindow = _serviceProvider.GetRequiredService<Window_Main>();
        mainWindow.ShowDialog();
    }


    private static void ConfigureServices(IServiceCollection services)
    {
        // Register ViewModels
        services.AddSingleton<IViewModelMain          , ViewModelMain>();
        services.AddSingleton<IViewModelDownloadUpdate, ViewModelDownloadUpdate>();
        services.AddSingleton<IViewModelRemindLater   , ViewModelRemindLater>();
        services.AddSingleton<IViewModelUpdate        , ViewModelUpdate>();

        // Register Views
        services.AddSingleton<Window_Main>();
        services.AddSingleton<Window_AutoUpdater>();
        services.AddSingleton<Window_DownloadUpdate>();
        services.AddSingleton<Window_Update>();
    }


    private void OnExit(object sender, ExitEventArgs e)
    {
        // Dispose of services if needed
        if (_serviceProvider is IDisposable disposable)
            disposable.Dispose();
    }
}