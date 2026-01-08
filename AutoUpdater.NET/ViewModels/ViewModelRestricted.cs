// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelRestricted.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

public abstract class ViewModelRestricted : ObservableObject
{
    public ViewModelMainConfig Config { get; }

    public Window? Owner { get; set; }


    /// <summary>
    /// Constructor
    /// </summary>
    protected ViewModelRestricted(ViewModelConfig vm)
    {
        Config = vm;
    }
}