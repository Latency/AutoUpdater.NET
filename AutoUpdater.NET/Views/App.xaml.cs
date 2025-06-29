// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     App.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Windows;

namespace AutoUpdaterDotNET.Views;

public partial class App
{
    protected override void OnStartup(StartupEventArgs? e) => AutoUpdate.Instance.ShowDialog();
}