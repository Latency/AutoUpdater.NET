// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     06/18/2025
// ****************************************************************************

using AssemblyLoader;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Views;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoUpdaterDotNET.ViewModels;

#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
public class ViewModelMainConfig : DependencyObject, IEquatable<ViewModelMainConfig>
#pragma warning restore CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private readonly Version _defaultInstalledVersion;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Dependancy Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public static readonly DependencyProperty ProxyUriProperty                      = DependencyProperty.Register(nameof(ProxyUri),                      typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty AppTitleProperty                      = DependencyProperty.Register(nameof(AppTitle),                      typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty BasicAuthPasswordProperty             = DependencyProperty.Register(nameof(BasicAuthPassword),             typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty BasicAuthUsernameProperty             = DependencyProperty.Register(nameof(BasicAuthUsername),             typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty ExecutablePathProperty                = DependencyProperty.Register(nameof(ExecutablePath),                typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty ImageUriProperty                      = DependencyProperty.Register(nameof(ImageUri),                      typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty InstallationPathProperty              = DependencyProperty.Register(nameof(InstallationPath),              typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty ProxyPasswordProperty                 = DependencyProperty.Register(nameof(ProxyPassword),                 typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty ProxyUsernameProperty                 = DependencyProperty.Register(nameof(ProxyUsername),                 typeof(string),                             typeof(Window_Main));
    public static readonly DependencyProperty IsManditoryProperty                   = DependencyProperty.Register(nameof(IsManditory),                   typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty ShowSkipButtonProperty                = DependencyProperty.Register(nameof(ShowSkipButton),                typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty ShowRemindLaterButtonProperty         = DependencyProperty.Register(nameof(ShowRemindLaterButton),         typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty RunUpdateAsAdminProperty              = DependencyProperty.Register(nameof(RunUpdateAsAdmin),              typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty OpenDownloadPageProperty              = DependencyProperty.Register(nameof(OpenDownloadPage),              typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty LetUserSelectRemindLaterProperty      = DependencyProperty.Register(nameof(LetUserSelectRemindLater),      typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty BasicAuthProperty                     = DependencyProperty.Register(nameof(BasicAuth),                     typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty BasicAuthChangeLogProperty            = DependencyProperty.Register(nameof(BasicAuthChangeLog),            typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty BasicAuthDownloadProperty             = DependencyProperty.Register(nameof(BasicAuthDownload),             typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty DoNotBindOwnerWindowProperty          = DependencyProperty.Register(nameof(DoNotBindOwnerWindow),          typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty ChangeUpdateZipExtractionPathProperty = DependencyProperty.Register(nameof(ChangeUpdateZipExtractionPath), typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty CheckSynchronouslyProperty            = DependencyProperty.Register(nameof(CheckSynchronously),            typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty ClearAppDirectoryProperty             = DependencyProperty.Register(nameof(ClearAppDirectory),             typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty ExecutablePathOverrideProperty        = DependencyProperty.Register(nameof(ExecutablePathOverride),        typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty FtpProtocolProperty                   = DependencyProperty.Register(nameof(FtpProtocol),                   typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty IconOverrideProperty                  = DependencyProperty.Register(nameof(IconOverride),                  typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty InstalledVersionOverrideProperty      = DependencyProperty.Register(nameof(InstalledVersionOverride),      typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty PersistSettingsProperty               = DependencyProperty.Register(nameof(PersistSettings),               typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty ProxyEnabledProperty                  = DependencyProperty.Register(nameof(ProxyEnabled),                  typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty ReportErrorsProperty                  = DependencyProperty.Register(nameof(ReportErrors),                  typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty TimerEnabledProperty                  = DependencyProperty.Register(nameof(TimerEnabled),                  typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty TopMostDisabledProperty               = DependencyProperty.Register(nameof(TopMostDisabled),               typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty UseZipFileProperty                    = DependencyProperty.Register(nameof(UseZipFile),                    typeof(bool),                               typeof(Window_Main));
    public static readonly DependencyProperty UpdateModeProperty                    = DependencyProperty.Register(nameof(UpdateMode),                    typeof(Mode),                               typeof(Window_Main));
    public static readonly DependencyProperty RemindLaterTimeSpanProperty           = DependencyProperty.Register(nameof(RemindLaterTimeSpan),           typeof(RemindLaterFormat),                  typeof(Window_Main));
    public static readonly DependencyProperty TimerDurationTimeSpanProperty         = DependencyProperty.Register(nameof(TimerDurationTimeSpan),         typeof(RemindLaterFormat),                  typeof(Window_Main));
    public static readonly DependencyProperty IntervalProperty                      = DependencyProperty.Register(nameof(Interval),                      typeof(ushort),                             typeof(Window_Main));
    public static readonly DependencyProperty MajorVersionProperty                  = DependencyProperty.Register(nameof(MajorVersion),                  typeof(ushort),                             typeof(Window_Main), new PropertyMetadata((ushort) typeof(ViewModelMainConfig).Assembly.Version()!.Major));
    public static readonly DependencyProperty MinorVersionProperty                  = DependencyProperty.Register(nameof(MinorVersion),                  typeof(ushort),                             typeof(Window_Main), new PropertyMetadata((ushort) typeof(ViewModelMainConfig).Assembly.Version()!.Minor));
    public static readonly DependencyProperty RevisionVersionProperty               = DependencyProperty.Register(nameof(RevisionVersion),               typeof(ushort),                             typeof(Window_Main), new PropertyMetadata((ushort) typeof(ViewModelMainConfig).Assembly.Version()!.Build));
    public static readonly DependencyProperty BuildVersionProperty                  = DependencyProperty.Register(nameof(BuildVersion),                  typeof(ushort),                             typeof(Window_Main), new PropertyMetadata((ushort) typeof(ViewModelMainConfig).Assembly.Version()!.Revision));
    public static readonly DependencyProperty RemindLaterAtProperty                 = DependencyProperty.Register(nameof(RemindLaterAt),                 typeof(ushort),                             typeof(Window_Main));
    public static readonly DependencyProperty TimerNodeListProperty                 = DependencyProperty.Register(nameof(TimerNodeList),                 typeof(ObservableCollection<TreeViewItem>), typeof(Window_Main), new PropertyMetadata((ObservableCollection<TreeViewItem>)[]));
    public static readonly DependencyProperty ApplicationExitNodeListProperty       = DependencyProperty.Register(nameof(ApplicationExitNodeList),       typeof(ObservableCollection<TreeViewItem>), typeof(Window_Main), new PropertyMetadata((ObservableCollection<TreeViewItem>)[]));
    public static readonly DependencyProperty CheckForUpdatesNodeListProperty       = DependencyProperty.Register(nameof(CheckForUpdatesNodeList),       typeof(ObservableCollection<TreeViewItem>), typeof(Window_Main), new PropertyMetadata((ObservableCollection<TreeViewItem>)[]));
    public static readonly DependencyProperty ParseUpdateInfoNodeListProperty       = DependencyProperty.Register(nameof(ParseUpdateInfoNodeList),       typeof(ObservableCollection<TreeViewItem>), typeof(Window_Main), new PropertyMetadata((ObservableCollection<TreeViewItem>)[]));
    public static readonly DependencyProperty IconProperty                          = DependencyProperty.Register(nameof(TmpIcon),                       typeof(ImageSource),                        typeof(Window_Main));
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Dependancy Properties


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    [Url, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ProxyUri
    {
        get => (string)GetValue(ProxyUriProperty)!;
        set => SetValue(ProxyUriProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string AppTitle
    {
        get => (string)GetValue(AppTitleProperty)!;
        set => SetValue(AppTitleProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string BasicAuthPassword
    {
        get => (string)GetValue(BasicAuthPasswordProperty)!;
        set => SetValue(BasicAuthPasswordProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string BasicAuthUsername
    {
        get => (string)GetValue(BasicAuthUsernameProperty)!;
        set => SetValue(BasicAuthUsernameProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ExecutablePath
    {
        get => (string)GetValue(ExecutablePathProperty)!;
        set => SetValue(ExecutablePathProperty, value);
    }

    [Url, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ImageUri
    {
        get => (string)GetValue(ImageUriProperty)!;
        private set => SetValue(ImageUriProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string InstallationPath
    {
        get => (string)GetValue(InstallationPathProperty)!;
        set => SetValue(InstallationPathProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ProxyPassword
    {
        get => (string)GetValue(ProxyPasswordProperty)!;
        set => SetValue(ProxyPasswordProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ProxyUsername
    {
        get => (string)GetValue(ProxyUsernameProperty)!;
        set => SetValue(ProxyUsernameProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsManditory
    {
        get => (bool)GetValue(IsManditoryProperty);
        set => SetValue(IsManditoryProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ShowSkipButton
    {
        get => (bool)GetValue(ShowSkipButtonProperty);
        set => SetValue(ShowSkipButtonProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ShowRemindLaterButton
    {
        get => (bool)GetValue(ShowRemindLaterButtonProperty);
        set => SetValue(ShowRemindLaterButtonProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool RunUpdateAsAdmin
    {
        get => (bool)GetValue(RunUpdateAsAdminProperty);
        set => SetValue(RunUpdateAsAdminProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool OpenDownloadPage
    {
        get => (bool)GetValue(OpenDownloadPageProperty);
        set => SetValue(OpenDownloadPageProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool LetUserSelectRemindLater
    {
        get => (bool)GetValue(LetUserSelectRemindLaterProperty);
        set => SetValue(LetUserSelectRemindLaterProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool BasicAuth
    {
        get => (bool)GetValue(BasicAuthProperty);
        set => SetValue(BasicAuthProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool BasicAuthChangeLog
    {
        get => (bool)GetValue(BasicAuthChangeLogProperty);
        set => SetValue(BasicAuthChangeLogProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool BasicAuthDownload
    {
        get => (bool)GetValue(BasicAuthDownloadProperty);
        set => SetValue(BasicAuthDownloadProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool DoNotBindOwnerWindow
    {
        get => (bool)GetValue(DoNotBindOwnerWindowProperty);
        set => SetValue(DoNotBindOwnerWindowProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ChangeUpdateZipExtractionPath
    {
        get => (bool)GetValue(ChangeUpdateZipExtractionPathProperty);
        set => SetValue(ChangeUpdateZipExtractionPathProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool CheckSynchronously
    {
        get => (bool)GetValue(CheckSynchronouslyProperty);
        set => SetValue(CheckSynchronouslyProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ClearAppDirectory
    {
        get => (bool)GetValue(ClearAppDirectoryProperty);
        set => SetValue(ClearAppDirectoryProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ExecutablePathOverride
    {
        get => (bool)GetValue(ExecutablePathOverrideProperty);
        set => SetValue(ExecutablePathOverrideProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool FtpProtocol
    {
        get => (bool)GetValue(FtpProtocolProperty);
        set => SetValue(FtpProtocolProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IconOverride
    {
        get => (bool)GetValue(IconOverrideProperty);
        set => SetValue(IconOverrideProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool PersistSettings
    {
        get => (bool)GetValue(PersistSettingsProperty);
        set => SetValue(PersistSettingsProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ProxyEnabled
    {
        get => (bool)GetValue(ProxyEnabledProperty);
        set => SetValue(ProxyEnabledProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool ReportErrors
    {
        get => (bool)GetValue(ReportErrorsProperty);
        set => SetValue(ReportErrorsProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool TimerEnabled
    {
        get => (bool)GetValue(TimerEnabledProperty);
        set => SetValue(TimerEnabledProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool TopMostDisabled
    {
        get => (bool)GetValue(TopMostDisabledProperty);
        set => SetValue(TopMostDisabledProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool UseZipFile
    {
        get => (bool)GetValue(UseZipFileProperty);
        set => SetValue(UseZipFileProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Mode UpdateMode
    {
        get => (Mode)GetValue(UpdateModeProperty);
        set => SetValue(UpdateModeProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RemindLaterFormat RemindLaterTimeSpan
    {
        get => (RemindLaterFormat)GetValue(RemindLaterTimeSpanProperty);
        set => SetValue(RemindLaterTimeSpanProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RemindLaterFormat TimerDurationTimeSpan
    {
        get => (RemindLaterFormat)GetValue(TimerDurationTimeSpanProperty);
        set => SetValue(TimerDurationTimeSpanProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ushort Interval
    {
        get => (ushort)GetValue(IntervalProperty);
        set => SetValue(IntervalProperty, value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ushort RemindLaterAt
    {
        get => (ushort)GetValue(RemindLaterAtProperty);
        set => SetValue(RemindLaterAtProperty, value);
    }

    [JsonIgnore]
    public bool InstalledVersionOverride
    {
        get => (bool)GetValue(InstalledVersionOverrideProperty);
        set
        {
            InstalledVersion = value ? new InstalledVersion { Version = _defaultInstalledVersion } : null;
            SetValue(InstalledVersionOverrideProperty, value);
        }
    }

    [JsonIgnore]
    public ushort MajorVersion
    {
        get => (ushort)GetValue(MajorVersionProperty);
        set => SetValue(MajorVersionProperty, value);
    }

    [JsonIgnore]
    public ushort MinorVersion
    {
        get => (ushort)GetValue(MinorVersionProperty);
        set
        {
            if (_defaultInstalledVersion.ToString() == $"{MajorVersion}.{value}.{BuildVersion}.{RevisionVersion}")
                InstalledVersion = null;
            else
            {
                InstalledVersion = new InstalledVersion
                {
                    Version = new Version(MajorVersion, value, BuildVersion, RevisionVersion)
                };
            }
            SetValue(MinorVersionProperty, value);
        }
    }

    [JsonIgnore]
    public ushort BuildVersion
    {
        get => (ushort)GetValue(BuildVersionProperty);
        set => SetValue(BuildVersionProperty, value);
    }

    [JsonIgnore]
    public ushort RevisionVersion
    {
        get => (ushort)GetValue(RevisionVersionProperty);
        set => SetValue(RevisionVersionProperty, value);
    }

    [JsonIgnore]
    public InstalledVersion? InstalledVersion
    {
        get;
        set;
    }

    [JsonIgnore]
    public ImageSource? TmpIcon
    {
        get => (ImageSource?)GetValue(IconProperty);
        set
        {
            if (value is not null)
                SetValue(IconProperty, value);

            ImageUri = value?.ToString() ?? string.Empty;
        }
    }

    [JsonIgnore]
    public ObservableCollection<TreeViewItem> TimerNodeList
    {
        get => (ObservableCollection<TreeViewItem>)GetValue(TimerNodeListProperty)!;
        set => SetValue(TimerNodeListProperty, value);
    }

    [JsonIgnore]
    public ObservableCollection<TreeViewItem> ApplicationExitNodeList
    {
        get => (ObservableCollection<TreeViewItem>)GetValue(ApplicationExitNodeListProperty)!;
        set => SetValue(ApplicationExitNodeListProperty, value);
    }

    [JsonIgnore]
    public ObservableCollection<TreeViewItem> CheckForUpdatesNodeList
    {
        get => (ObservableCollection<TreeViewItem>)GetValue(CheckForUpdatesNodeListProperty)!;
        set => SetValue(CheckForUpdatesNodeListProperty, value);
    }

    [JsonIgnore]
    public ObservableCollection<TreeViewItem> ParseUpdateInfoNodeList
    {
        get => (ObservableCollection<TreeViewItem>)GetValue(ParseUpdateInfoNodeListProperty)!;
        set => SetValue(ParseUpdateInfoNodeListProperty, value);
    }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties



    /// <summary>
    ///     Default Constructor
    /// </summary>
    public ViewModelMainConfig()
    {
        _defaultInstalledVersion = new Version(MajorVersion, MinorVersion, BuildVersion, RevisionVersion);
    }


    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="other"></param>
    public ViewModelMainConfig(ViewModelMainConfig? other) : this()
    {
        if (other is null)
            return;

        ProxyUri                      = other.ProxyUri;
        AppTitle                      = other.AppTitle;
        BasicAuthPassword             = other.BasicAuthPassword;
        BasicAuthUsername             = other.BasicAuthUsername;
        ExecutablePath                = other.ExecutablePath;
        ImageUri                      = other.ImageUri;
        InstallationPath              = other.InstallationPath;
        ProxyPassword                 = other.ProxyPassword;
        ProxyUsername                 = other.ProxyUsername;
        ShowSkipButton                = other.ShowSkipButton;
        IsManditory                   = other.IsManditory;
        ShowRemindLaterButton         = other.ShowRemindLaterButton;
        RunUpdateAsAdmin              = other.RunUpdateAsAdmin;
        LetUserSelectRemindLater      = other.LetUserSelectRemindLater;
        OpenDownloadPage              = other.OpenDownloadPage;
        BasicAuth                     = other.BasicAuth;
        BasicAuthChangeLog            = other.BasicAuthChangeLog;
        DoNotBindOwnerWindow          = other.DoNotBindOwnerWindow;
        BasicAuthDownload             = other.BasicAuthDownload;
        CheckSynchronously            = other.CheckSynchronously;
        ChangeUpdateZipExtractionPath = other.ChangeUpdateZipExtractionPath;
        ExecutablePathOverride        = other.ExecutablePathOverride;
        ClearAppDirectory             = other.ClearAppDirectory;
        IconOverride                  = other.IconOverride;
        FtpProtocol                   = other.FtpProtocol;
        PersistSettings               = other.PersistSettings;
        InstalledVersionOverride      = other.InstalledVersionOverride;
        ReportErrors                  = other.ReportErrors;
        ProxyEnabled                  = other.ProxyEnabled;
        TimerEnabled                  = other.TimerEnabled;
        TopMostDisabled               = other.TopMostDisabled;
        UseZipFile                    = other.UseZipFile;
        UpdateMode                    = other.UpdateMode;
        RemindLaterTimeSpan           = other.RemindLaterTimeSpan;
        TimerDurationTimeSpan         = other.TimerDurationTimeSpan;
        Interval                      = other.Interval;
        RemindLaterAt                 = other.RemindLaterAt;
    }


    public bool Equals(ViewModelMainConfig? other)
    {
        if (other == null)
            return false;

        return (
                   ReferenceEquals(ProxyUri, other.ProxyUri) ||
                   ProxyUri == other.ProxyUri
               ) &&
               (
                   ReferenceEquals(AppTitle, other.AppTitle) ||
                   AppTitle == other.AppTitle
               ) &&
               (
                   ReferenceEquals(BasicAuthPassword, other.BasicAuthPassword) ||
                   BasicAuthPassword == other.BasicAuthPassword
               ) &&
               (
                   ReferenceEquals(BasicAuthUsername, other.BasicAuthUsername) ||
                   BasicAuthUsername == other.BasicAuthUsername
               ) &&
               (
                   ReferenceEquals(ExecutablePath, other.ExecutablePath) ||
                   ExecutablePath == other.ExecutablePath
               ) &&
               (
                   ReferenceEquals(ImageUri, other.ImageUri) ||
                   ImageUri == other.ImageUri
               ) &&
               (
                   ReferenceEquals(InstallationPath, other.InstallationPath) ||
                   InstallationPath == other.InstallationPath
               ) &&
               (
                   ReferenceEquals(ProxyPassword, other.ProxyPassword) ||
                   ProxyPassword == other.ProxyPassword
               ) &&
               (
                   ReferenceEquals(ProxyUsername, other.ProxyUsername) ||
                   ProxyUsername == other.ProxyUsername
               ) &&
               (
                   InstalledVersionOverride && (
                   ReferenceEquals(InstalledVersion, other.InstalledVersion) ||
                   InstalledVersion == other.InstalledVersion)
               )                                                                    &&
               ShowSkipButton                == other.ShowSkipButton                &&
               IsManditory                   == other.IsManditory                   &&
               ShowRemindLaterButton         == other.ShowRemindLaterButton         &&
               RunUpdateAsAdmin              == other.RunUpdateAsAdmin              &&
               LetUserSelectRemindLater      == other.LetUserSelectRemindLater      &&
               OpenDownloadPage              == other.OpenDownloadPage              &&
               BasicAuth                     == other.BasicAuth                     &&
               BasicAuthChangeLog            == other.BasicAuthChangeLog            &&
               DoNotBindOwnerWindow          == other.DoNotBindOwnerWindow          &&
               BasicAuthDownload             == other.BasicAuthDownload             &&
               CheckSynchronously            == other.CheckSynchronously            &&
               ChangeUpdateZipExtractionPath == other.ChangeUpdateZipExtractionPath &&
               ExecutablePathOverride        == other.ExecutablePathOverride        &&
               ClearAppDirectory             == other.ClearAppDirectory             &&
               IconOverride                  == other.IconOverride                  &&
               FtpProtocol                   == other.FtpProtocol                   &&
               PersistSettings               == other.PersistSettings               &&
               ReportErrors                  == other.ReportErrors                  &&
               ProxyEnabled                  == other.ProxyEnabled                  &&
               TimerEnabled                  == other.TimerEnabled                  &&
               TopMostDisabled               == other.TopMostDisabled               &&
               UseZipFile                    == other.UseZipFile                    &&
               UpdateMode                    == other.UpdateMode                    &&
               RemindLaterTimeSpan           == other.RemindLaterTimeSpan           &&
               TimerDurationTimeSpan         == other.TimerDurationTimeSpan         &&
               Interval                      == other.Interval                      &&
               MajorVersion                  == other.MajorVersion                  &&
               MinorVersion                  == other.MinorVersion                  &&
               RevisionVersion               == other.RevisionVersion               &&
               BuildVersion                  == other.BuildVersion                  &&
               RemindLaterAt                 == other.RemindLaterAt;
    }
}