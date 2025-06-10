// ****************************************************************************
// Project:  BHI
// File:     IViewModelMain.cs
// Author:   Latency McLaughlin
// Date:     04/18/2025
// ****************************************************************************
// ReSharper disable UnusedMemberInSuper.Global

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AutoUpdaterDotNET.Enums;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelMain : INotifyCollectionChanged
{
    event AutoUpdater.ApplicationExitEventHandler? ApplicationExit;
    event AutoUpdater.CheckForUpdateEventHandler?  CheckForUpdates;
    event AutoUpdater.ParseUpdateInfoHandler?      ParseUpdateInfo;

    void ButtonCancel_Click(object?     sender);
    void ButtonUpdate_Click(object?     sender);
    void ButtonSaveConfig_Click(object? sender);
    void ButtonLoadConfig_Click(object? sender);
    void Image_Click(object?            sender);

    ICommand CommandCancel      { get; set; }
    ICommand CommandUpdate      { get; set; }
    ICommand CommandSaveConfig  { get; set; }
    ICommand CommandLoadConfig  { get; set; }
    ICommand CommandImageChange { get; set; }


    bool                               IsManditory                   { get; set; }
    bool                               ShowSkipButton                { get; set; }
    bool                               ShowRemindLaterButton         { get; set; }
    bool                               RunUpdateAsAdmin              { get; set; }
    bool                               OpenDownloadPage              { get; set; }
    bool                               LetUserSelectRemindLater      { get; set; }
    RemindLaterFormat                  RemindLaterTimeSpan           { get; set; }
    ushort                             RemindLaterAt                 { get; set; }
    string                             AppTitle                      { get; set; }
    bool                               ReportErrors                  { get; set; }
    bool                               TimerEnabled                  { get; set; }
    RemindLaterFormat                  TimerDurationTimeSpan         { get; set; }
    ushort                             Interval                      { get; set; }
    ObservableCollection<TreeViewItem> TimerNodeList                 { get; set; }
    ObservableCollection<TreeViewItem> ApplicationExitNodeList       { get; set; }
    ObservableCollection<TreeViewItem> CheckForUpdatesNodeList       { get; set; }
    ObservableCollection<TreeViewItem> ParseUpdateInfoNodeList       { get; set; }
    bool                               ProxyEnabled                  { get; set; }
    string                             ProxyUri                      { get; set; }
    string                             ProxyUsername                 { get; set; }
    string                             ProxyPassword                 { get; set; }
    Mode                               UpdateMode                    { get; set; }
    bool                               BasicAuth                     { get; set; }
    bool                               BasicAuthChangeLog            { get; set; }
    bool                               BasicAuthDownload             { get; set; }
    string                             BasicAuthUsername             { get; set; }
    string                             BasicAuthPassword             { get; set; }
    bool                               FTPProtocol                   { get; set; }
    bool                               PersistSettings               { get; set; }
    bool                               UseZipFile                    { get; set; }
    bool                               ChangeUpdateZipExtractionPath { get; set; }
    string                             InstallationPath              { get; set; }
    bool                               CheckSynchronously            { get; set; }
    bool                               InstalledVersionOverride      { get; set; }
    ushort                             MajorVersion                  { get; set; }
    ushort                             MinorVersion                  { get; set; }
    ushort                             SubPatchVersion               { get; set; }
    ushort                             BuildVersion                  { get; set; }
    bool                               ClearAppDirectory             { get; set; }
    bool                               ExecutablePathOverride        { get; set; }
    string                             ExecutablePath                { get; set; }
    bool                               BindOwnerWindow               { get; set; }
    bool                               TopMostEnabled                { get; set; }
    bool                               IconOverride                  { get; set; }
    ImageSource                        TmpIcon                       { get; }
    string                             ImageUri                      { get; set; }
}