// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelMain.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable UnusedMemberInSuper.Global

using AutoUpdaterDotNET.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelMain : INotifyPropertyChanged
{
    ICommand CommandCancel      { get; set; }
    ICommand CommandUpdate      { get; set; }
    ICommand CommandSaveConfig  { get; set; }
    ICommand CommandLoadConfig  { get; set; }
    ICommand CommandImageChange { get; set; }


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    ObservableCollection<TreeViewItem> TimerNodeList           { get; set; }
    ObservableCollection<TreeViewItem> ApplicationExitNodeList { get; set; }
    ObservableCollection<TreeViewItem> CheckForUpdatesNodeList { get; set; }
    ObservableCollection<TreeViewItem> ParseUpdateInfoNodeList { get; set; }
    ImageSource?                       TmpIcon                 { get; set; }
    DispatcherTimer                    UpdateTimer             { get; }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Delegates
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    /// <summary>
    ///     A delegate type to handle how to exit the application after update is downloaded.
    /// </summary>
    delegate void ApplicationExitEventHandler();

    /// <summary>
    ///     A delegate type for hooking up update notifications.
    /// </summary>
    /// <param name="args">
    ///     An object containing all the parameters received from AppCast XML file. If there will be an error
    ///     while looking for the XML file then this object will be null.
    /// </param>
    delegate void CheckForUpdateEventHandler(UpdateInfoEventArgs args);

    /// <summary>
    ///     A delegate type for hooking up parsing logic.
    /// </summary>
    /// <param name="args">An object containing the AppCast file received from server.</param>
    delegate void ParseUpdateInfoHandler(ParseUpdateInfoEventArgs args);

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Delegates


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    event ApplicationExitEventHandler? ApplicationExit;
    event CheckForUpdateEventHandler?  CheckForUpdates;
    event ParseUpdateInfoHandler?      ParseUpdateInfo;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    #region Methods
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    void              ButtonCancel_Click(object?     sender);
    void              ButtonUpdate_Click(object?     sender);
    void              ButtonSaveConfig_Click(object? sender);
    void              ButtonLoadConfig_Click(object? sender);
    void              Image_Click(object?            sender);
    void              OnLoaded(object?               sender);

    bool              Equals();
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #endregion Methods
}