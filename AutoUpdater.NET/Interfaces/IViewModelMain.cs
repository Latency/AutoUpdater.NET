// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelMain.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable UnusedMemberInSuper.Global

using System.Windows.Media;
using AutoUpdaterDotNET.Models;
using System.Windows.Threading;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelMain : IViewModelMainConfig
{
    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    DispatcherTimer UpdateTimer { get; }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    event Action?                           ApplicationExit;
    event Action<UpdateInfoEventArgs>?      CheckForUpdates;
    event Action<ParseUpdateInfoEventArgs>? ParseUpdateInfo;
    event Action<string?>                   UpdateVersion;
    event Action<ImageSource?>?             UpdateIcon;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    #region Methods
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    void OnLoaded(object? sender);

    bool Equals();
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}