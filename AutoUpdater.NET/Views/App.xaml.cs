// ****************************************************************************
// Project:  BHI
// File:     App.xaml.cs
// Author:   Latency McLaughlin
// Date:     04/18/2025
// ****************************************************************************

using System.Reflection;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AutoUpdaterDotNET.Views;

public class AutoUpdate
{
    private static IServiceProvider _serviceProvider = null!;

    internal static T GetWindow<T>(object? serviceKey) where T : class => (_serviceProvider ?? throw new InvalidOperationException()).GetRequiredKeyedService<T>(serviceKey);


    private static readonly Lazy<AutoUpdate> SingletonInstance = new(() =>
    {
        var au                = new AutoUpdate();
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();

        return au;
    });
    public static AutoUpdate Instance => SingletonInstance.Value;


    // Singleton Constructor
    private AutoUpdate()
    { }


    private static void ConfigureServices(IServiceCollection services)
    {
        // Register ViewModels
        services.AddSingleton<IViewModelMain, ViewModelMain>();
        services.AddSingleton<IViewModelDownloadUpdate, ViewModelDownloadUpdate>();
        services.AddSingleton<IViewModelRemindLater, ViewModelRemindLater>();
        services.AddSingleton<IViewModelUpdate, ViewModelUpdate>();

        // Register Views
        services.AddSingleton<Window_Main>();
        services.AddSingleton<Window_DownloadUpdate>();
        services.AddSingleton<Window_RemindLater>();
        services.AddSingleton<Window_Update>();
    }


#pragma warning disable CA1822
    public void LoadResources()
#pragma warning restore CA1822
    {
        var asm = Assembly.GetAssembly(GetType())!;

        foreach (var file in new[]
        {
            "Images",
            "Styles"
        })
        {
            var uriPath = $"pack://application:,,,/{asm.GetName().Name};component/Resources/{file}.xaml";
            var myResourceDictionary = new ResourceDictionary
            {
                Source = new Uri(uriPath, UriKind.Absolute)
            };
            Application.Current!.Resources.MergedDictionaries.Add(myResourceDictionary);
        }
    }


#pragma warning disable CA1822
    public void ShowDialog()
#pragma warning restore CA1822
    {
        var mainWindow = _serviceProvider.GetRequiredService<Window_Main>();
        mainWindow.Show();
    }
}



public partial class App
{
    protected override void OnStartup(StartupEventArgs? e) => AutoUpdate.Instance.ShowDialog();
}