// ****************************************************************************
// Project:  WpfApp1
// File:     App.xaml.cs
// Author:   Latency McLaughlin
// Date:     10/14/2025
// ****************************************************************************

using System.Windows;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.ViewModels;
using AutoUpdaterDotNET.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WpfApp1.Extensions;
using WpfApp1.Interfaces;
using WpfApp1.View_Models;
using WpfApp1.Views;

namespace WpfApp1;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
                    .AddViewModels()
                    .ConfigureServices((hostContext, services) =>
                    {
                        var db = hostContext.Configuration.GetConnectionString("Default");

                        services.AddSingleton<IViewModelMain, ViewModelMain>();
                        services.AddSingleton(s => new Window_Main
                        {
                            DataContext = s.GetRequiredService<ViewModelMain>()
                        });

                        services.AddSingleton<IMainViewModel, MainViewModel>();
                        services.AddSingleton(s => new MainWindow
                        {
                            DataContext = s.GetRequiredService<MainViewModel>()
                        });
                    })
                    .ConfigureLogging(logging =>
                    {
                        // Configure logging providers
                        logging.ClearProviders(); // Clear default providers
                        logging.AddConsole();     // Add console logging
                        logging.AddDebug();       // Add debug logging
                    })
                    .Build();
    }


    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        // Resolve and show your main window
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }


    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
        base.OnExit(e);
    }
}