// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_RemindLater.xaml.cs
// Author:   Latency McLaughlin
// Date:     05/17/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Windows;
using System.Windows.Controls;
using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Views;

/// <summary>
///     Interaction logic for RemindLater.xaml
/// </summary>
public sealed partial class Window_RemindLater : RestrictedWindow
{
    public Window_RemindLater(IViewModelRemindLater vm)
    {
        InitializeComponent();

        DataContext = vm;

        if (ButtonOk != null)
            ButtonOk.Click += (sender, e) => vm.CommandButtonOk.Execute(new Tuple<Window_RemindLater, Button?, RoutedEventArgs>(this, sender as Button, e));
    }
}