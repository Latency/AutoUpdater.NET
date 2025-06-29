// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     App.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Reflection;
using System.Windows;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.ViewModels;
using AutoUpdaterDotNET.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AutoUpdaterDotNET;

public sealed class AutoUpdate
{
    private static IServiceProvider _serviceProvider = null!;
    public static  AutoUpdate Instance => SingletonInstance.Value;

    #pragma warning disable CA2211
    public static Action? OnInitialize;
    #pragma warning restore CA2211


    private static readonly Lazy<AutoUpdate> SingletonInstance = new(() =>
    {
        var au                = new AutoUpdate();
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        _serviceProvider = serviceCollection.BuildServiceProvider();

        OnInitialize?.Invoke();

        return au;

        static void ConfigureServices(IServiceCollection services)
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
    });


    /// <summary>
    ///     Singleton Constructor
    /// </summary>
    private AutoUpdate()
    { }


    /// <summary>
    ///     GetWindow
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serviceKey"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    internal static T GetWindow<T>(object? serviceKey)
        where T : class => (_serviceProvider ?? throw new InvalidOperationException()).GetRequiredKeyedService<T>(serviceKey);


    /// <summary>
    ///     LoadResources
    /// </summary>
    #pragma warning disable CA1822
    public static void LoadResources()
    #pragma warning restore CA1822
    {
        var asm = Assembly.GetAssembly(typeof(AutoUpdate))!;

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


    /// <summary>
    ///     ShowDialog
    /// </summary>
    /// <param name="win"></param>
    #pragma warning disable CA1822
    public void ShowDialog(Window? win = null)
    #pragma warning restore CA1822
    {
        var mainWindow = _serviceProvider.GetRequiredService<Window_Main>();
        if (win is not null)
            mainWindow.Owner = win;

        try
        {
            win?.Hide();
            mainWindow.ShowDialog();
        }
        finally
        {
            win?.Show();
        }
    }
}