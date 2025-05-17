// ****************************************************************************
// Project:  Patch1
// File:     Window_Main.xaml.cs
// Author:   Latency McLaughlin
// Date:     04/16/2024
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Reflection;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.ViewModels;

namespace AutoUpdaterDotNET.Views;

/// <summary>
///     Interaction logic for Window_Main.xaml
/// </summary>
public partial class Window_Main
{
    private readonly ViewModelMain vm;


    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_Main(IViewModelMain viewModel)
    {
        if (viewModel is null)
            throw new NullReferenceException();

        if (viewModel is not ViewModelMain vm1)
            throw new ArgumentNullException(nameof(viewModel));

        InitializeComponent();

        DataContext = vm1;
        vm          = vm1;

        if (LabelVersion?.Content != null)
        {
            var format = LabelVersion.Content.ToString();
            var ver    = Assembly.GetEntryAssembly()!.GetName().Version!;
            LabelVersion.Content = string.Format(format!, $"{ver.Major}.{ver.Minor}.{ver.Build}");
        }
    }
}