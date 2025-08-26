// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     07/27/2025
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelMainConfig : IEquatable<ViewModelMainConfig>
{
    event Action<bool?>? UpdateValidation;
    void OnUpdateValidation(bool? isEnabled);

    string AppTitle              { get; set; }
    string BasicAuthPassword     { get; set; }
    string BasicAuthUserName     { get; set; }
    string ExecutablePath        { get; set; }
    string ImageUri              { get; }
    string InstallationPath      { get; set; }
    bool   IsManditory           { get; set; }
    bool   ShowSkipButton        { get; set; }
    bool   ShowRemindLaterButton { get; set; }
    bool   RunUpdateAsAdmin      { get; set; }
    bool   OpenDownloadPage      { get; set; }
    BasicAuth?                         BasicAuth                     { get; set; }
    bool                               BasicAuthChangeLog            { get; set; }
    bool                               BasicAuthDownload             { get; set; }
    bool                               DoNotBindOwnerWindow          { get; set; }
    bool                               ChangeUpdateZipExtractionPath { get; set; }
    bool                               CheckSynchronously            { get; set; }
    bool                               ClearAppDirectory             { get; set; }
    bool                               ExecutablePathOverride        { get; set; }
    bool                               FtpProtocol                   { get; set; }
    bool                               IsIconOverride                { get; set; }
    bool                               PersistSettings               { get; set; }
    bool                               ProxyEnabled                  { get; set; }
    bool                               ReportErrors                  { get; set; }
    bool                               TimerEnabled                  { get; set; }
    bool                               TopMostDisabled               { get; set; }
    bool                               UseZipFile                    { get; set; }
    Mode                               UpdateMode                    { get; set; }
    RemindLaterFormat                  TimerDurationTimeSpan         { get; set; }
    ushort                             TimerInterval                 { get; set; }
    bool                               UserSelectRemindLater         { get; set; }
    ushort                             RemindLaterAt                 { get; set; }
    RemindLaterFormat                  RemindLaterTimeSpan           { get; set; }
    bool                               InstalledVersionOverride      { get; set; }
    InstalledVersion?                  InstalledVersion              { get; set; }
    ProxyEnabled?                      Proxy                         { get; set; }
    ImageSource?                       TmpIcon                       { get; set; }
    ObservableCollection<TreeViewItem> TimerNodeList                 { get; set; }
    ObservableCollection<TreeViewItem> ApplicationExitNodeList       { get; set; }
    ObservableCollection<TreeViewItem> CheckForUpdatesNodeList       { get; set; }
    ObservableCollection<TreeViewItem> ParseUpdateInfoNodeList       { get; set; }
}