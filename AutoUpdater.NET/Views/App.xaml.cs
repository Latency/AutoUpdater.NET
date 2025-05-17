// ****************************************************************************
// Project:  BHI
// File:     App.xaml.cs
// Author:   Latency McLaughlin
// Date:     04/18/2025
// ****************************************************************************

using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.ViewModels;

namespace AutoUpdaterDotNET.Views;

public partial class App
{
    private IServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();

        var mainWindow = _serviceProvider.GetRequiredService<Window_Main>();
        mainWindow.Show();
    }


    private static void ConfigureServices(IServiceCollection services)
    {
        // Register ViewModels
        services.AddSingleton<IViewModelMain, ViewModelMain>();
        services.AddSingleton<IViewModelDownloadUpdate, ViewModelDownloadUpdate>();
        services.AddSingleton<IViewModelRemindLater, ViewModelRemindLater>();

        // Register Views
        services.AddSingleton<Window_Main>();
        services.AddSingleton<Window_DownloadUpdate>();
        services.AddSingleton<Window_RemindLater>();
        
    }
}