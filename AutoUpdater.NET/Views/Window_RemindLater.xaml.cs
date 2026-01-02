// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_RemindLater.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Controls;
using AutoUpdaterDotNET.ViewModels;

namespace AutoUpdaterDotNET.Views;

/// <summary>
///     Interaction logic for RemindLater.xaml
/// </summary>
public sealed partial class Window_RemindLater : RestrictedWindow
{
    public Window_RemindLater(ViewModelRemindLater vm)
    {
        InitializeComponent();
    }
}