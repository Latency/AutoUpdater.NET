// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.ViewModels;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelMainConfig
{
    bool IsAppTitle { get; set; }

    string? AppTitle { get; set; }

    bool IsManditory { get; set; }

    bool ShowSkipButton { get; set; }

    bool ShowRemindLaterButton { get; set; }

    Mode UpdateMode { get; set; }

    IsManditory? Manditory { get; set; }

    bool RunUpdateAsAdmin { get; set; }

    bool OpenDownloadPage { get; set; }

    BasicAuth? BasicAuth { get; set; }

    bool IsBasicAuth { get; set; }

    bool BasicAuthChangeLog { get; set; }

    bool BasicAuthDownload { get; set; }

    string? BasicAuthUserName { get; set; }

    string? BasicAuthPassword { get; set; }

    bool DoNotBindOwnerWindow { get; set; }

    bool CheckSynchronously { get; set; }

    bool FtpProtocol { get; set; }

    bool IsIconOverride { get; set; }

    IconOverride? IconOverride { get; set; }

    bool PersistSettings { get; set; }

    bool ProxyEnabled { get; set; }

    string? ProxyUri { get; set; }

    string? ProxyUserName { get; set; }

    string? ProxyPassword { get; set; }

    ProxyEnabled? Proxy { get; set; }

    bool ReportErrors { get; set; }

    bool TopMostDisabled { get; set; }

    bool UseZipFile { get; set; }

    bool ClearAppDirectory { get; set; }

    bool ExecutablePathOverride { get; set; }

    string? ExecutablePath { get; set; }

    bool ZipExtractionPathOverride { get; set; }

    string? InstallationPath { get; set; }

    ZipFile? ZipFile { get; set; }

    bool TimerEnabled { get; set; }

    ushort TimerInterval { get; set; }

    RemindLaterFormat TimerDurationTimeSpan { get; set; }

    TimerEnabled? Timer { get; set; }

    bool UserSelectRemindLater { get; set; }

    ushort RemindLaterAt { get; set; }

    RemindLaterFormat RemindLaterTimeSpan { get; set; }

    TimerEnabled? RemmindLaterTimer { get; set; }

    InstalledVersion? InstalledVersion { get; set; }

    bool InstalledVersionOverride { get; set; }

    ushort MajorVersion { get; set; }

    ushort MinorVersion { get; set; }

    ushort BuildVersion { get; set; }

    ushort RevisionVersion { get; set; }

    BitmapImage? TmpIcon { get; set; }

    ObservableCollection<TreeViewItem> TimerNodeList { get; set; }

    ObservableCollection<TreeViewItem> ApplicationExitNodeList { get; set; }

    ObservableCollection<TreeViewItem> CheckForUpdatesNodeList { get; set; }

    ObservableCollection<TreeViewItem> ParseUpdateInfoNodeList { get; set; }

    event Action<bool?>?                        UpdateValidation;
    event Action<string?>?                      UpdateVersion;
    event Action<BitmapImage?>?                 UpdateIcon;

    void                                        Update();
    bool                                        Equals();
    bool                                        Equals(ViewModelMainConfig? other);

    event PropertyChangedEventHandler?  PropertyChanged;
    event PropertyChangingEventHandler? PropertyChanging;
}