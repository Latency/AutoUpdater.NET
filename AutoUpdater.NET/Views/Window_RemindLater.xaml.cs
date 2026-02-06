// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_RemindLater.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.ComponentModel;
using AutoUpdaterDotNET.Controls;

namespace AutoUpdaterDotNET.Views;

public partial class Window_RemindLater : Window_Restricted
{
    /// <summary>
    /// Constructor
    /// </summary>
    public Window_RemindLater()
    {
        InitializeComponent();
    }


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected override void OnClosing(object? sender, CancelEventArgs e)
    {
        base.OnClosing(sender, e);
        Owner?.Show();
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}