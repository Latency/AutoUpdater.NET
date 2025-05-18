// ****************************************************************************
// Project:  Patch1
// File:     Window_Main.xaml.cs
// Author:   Latency McLaughlin
// Date:     04/16/2024
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Interfaces;
using System.Reflection;

namespace AutoUpdaterDotNET.Views;

/// <summary>
///     Interaction logic for Window_Main.xaml
/// </summary>
public sealed partial class Window_Main
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_Main(IViewModelMain vm)
    {
        InitializeComponent();

        DataContext = vm;

        // ReSharper disable once InvertIf
        if (LabelVersion?.Content != null)
        {
            var format = LabelVersion.Content.ToString();
            var ver    = Assembly.GetEntryAssembly()!.GetName().Version!;
            LabelVersion.Content = string.Format(format!, $"{ver.Major}.{ver.Minor}.{ver.Build}");
        }
    }
}