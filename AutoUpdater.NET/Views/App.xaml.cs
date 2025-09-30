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
using AutoUpdaterDotNET.Extensions;

namespace AutoUpdaterDotNET.Views;

public partial class App
{
    private static IServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs? e)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var resourceUri = new Uri("pack://application:,,,/png/Project.png");
        var streamInfo  = GetResourceStream(resourceUri);

        if (streamInfo is not null)
        {
            // <BitmapImage x: Key = "Project" UriSource = "pack://application:,,,/AutoUpdater.NET;component/png/project.png" />
            // Dynamic addition to 'Styles/Images.cs' targeting 'Project' as key.
            var imgResourceDict = Current!.Resources.MergedDictionaries[0]!;
            var bmp             = streamInfo.Stream!.ConvertStreamToBitmapImage(resourceUri);
            imgResourceDict.Add("Project", bmp);
        }

        _serviceProvider = serviceCollection.BuildServiceProvider();
        var mainFrm = _serviceProvider.GetRequiredService<Window_Main>();
        mainFrm.ShowDialog();
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