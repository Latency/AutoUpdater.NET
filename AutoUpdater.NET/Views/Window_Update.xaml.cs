// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Update.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.ComponentModel;
using AutoUpdaterDotNET.Controls;

namespace AutoUpdaterDotNET.Views;

public sealed partial class Window_Update : RestrictedWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_Update()
    {
        InitializeComponent();
    }


    private void Window_Update_OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}