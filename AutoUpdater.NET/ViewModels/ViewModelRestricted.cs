// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelRestricted.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Windows;
using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

/// <summary>
/// Constructor
/// </summary>
public abstract class ViewModelRestricted(BaseServiceDependencies dependencies) : ObservableObject, IViewModelRestricted
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected BaseServiceDependencies Dependencies = dependencies;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public IViewModelMainConfig Config => Dependencies.Config;

    Window? IViewModelRestricted.Owner { get; set; }

    public Window Window { get; set; } = null!;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties
}