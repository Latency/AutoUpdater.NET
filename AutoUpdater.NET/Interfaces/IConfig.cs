// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IConfig.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;

namespace AutoUpdaterDotNET.Interfaces;

public interface IConfig : IEquatable<IConfig?>
{
    FtpProfile2? FtpProfile { get; set; }

    bool WindowSizeOverride { get; set; }

    WindowSize? WindowSize { get; set; }

    bool  IsAppTitle { get; set; }

    string? AppTitle { get; set; }

    bool IsMandatory { get; set; }

    bool ShowSkipButton { get; set; }

    bool ShowRemindLaterButton { get; set; }

    Mode UpdateMode { get; set; }

    IsMandatory? Mandatory { get; set; }

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

    bool ProxyEnabled { get; set; }

    string? ProxyUri { get; set; }

    string? ProxyUserName { get; set; }

    string? ProxyPassword { get; set; }

    ProxyEnabled? Proxy { get; set; }

    bool ReportErrors { get; set; }

    bool TopMostDisabled { get; set; }

    CheckSum? CheckSum { get; set; }

    bool ClearAppDirectory { get; set; }

    bool TimerEnabled { get; set; }

    ushort TimerInterval { get; set; }

    RemindLaterFormat TimerDurationTimeSpan { get; set; }

    FilePath? ExecutablePathOverride { get; set; }

    FilePath? InstallationPathOverride { get; set; }

    TimerEnabled? Timer { get; set; }

    bool UserSelectRemindLater { get; set; }

    ushort RemindLaterAt { get; set; }

    RemindLaterFormat RemindLaterTimeSpan { get; set; }

    TimerEnabled? RemmindLaterTimer { get; set; }

    Version2? InstalledVersion { get; set; }

    bool InstalledVersionOverride { get; set; }

    BitmapImage? TmpIcon { get; set; }

    ObservableCollection<TreeViewItem>? TimerNodeList { get; set; }

    ObservableCollection<TreeViewItem>? UpdateCompleteNodeList  { get; set; }

    ObservableCollection<TreeViewItem>? BeforeCheckForUpdatesNodeList { get; set; }

    ObservableCollection<TreeViewItem>? AfterCheckForUpdatesNodeList { get; set; }
}