// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     06/18/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming
#pragma warning disable MVVMTK0042

using AssemblyLoader;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using System.Windows.Media;
using AutoUpdaterDotNET.Interfaces;


namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelMainConfig : ObservableObject, IViewModelMainConfig
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected Version _defaultInstalledVersion;

    private ProxyEnabled? _defaultProxy;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _appTitle;
    partial void OnAppTitleChanged(string? value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _basicAuthPassword;
    partial void OnBasicAuthPasswordChanged(string? value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _basicAuthUserName;
    partial void OnBasicAuthUserNameChanged(string? value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _executablePath;
    partial void OnExecutablePathChanged(string? value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _imageUri;
    partial void OnImageUriChanged(string? value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _installationPath;
    partial void OnInstallationPathChanged(string? value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _isManditory;
    partial void OnIsManditoryChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _showSkipButton;
    partial void OnShowSkipButtonChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _showRemindLaterButton;
    partial void OnShowRemindLaterButtonChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _runUpdateAsAdmin;
    partial void OnRunUpdateAsAdminChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _openDownloadPage;
    partial void OnOpenDownloadPageChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _basicAuth;
    partial void OnBasicAuthChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _basicAuthChangeLog;
    partial void OnBasicAuthChangeLogChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _basicAuthDownload;
    partial void OnBasicAuthDownloadChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _doNotBindOwnerWindow;
    partial void OnDoNotBindOwnerWindowChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _changeUpdateZipExtractionPath;
    partial void OnChangeUpdateZipExtractionPathChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _checkSynchronously;
    partial void OnCheckSynchronouslyChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _clearAppDirectory;
    partial void OnClearAppDirectoryChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _executablePathOverride;
    partial void OnExecutablePathOverrideChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _ftpProtocol;
    partial void OnFtpProtocolChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _iconOverride;
    partial void OnIconOverrideChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _persistSettings;
    partial void OnPersistSettingsChanged(bool value) => OnUpdateValidation();

    #region ProxyEnabled

    [ObservableProperty]
    [property: JsonIgnore]
    public bool _proxyEnabled;
    partial void OnProxyEnabledChanged(bool value) => Proxy = value ? _defaultProxy ??= new ProxyEnabled() : null;

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyUri;
    partial void OnProxyUriChanged(string? value)
    {
        if (Proxy is not null)
            Proxy.Uri = value;
        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyUserName;
    partial void OnProxyUserNameChanged(string? value)
    {
        if (Proxy is not null)
            Proxy.UserName = value;
        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyPassword;
    partial void OnProxyPasswordChanged(string? value)
    {
        if (Proxy is not null)
            Proxy.Password = value;
        OnUpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProxyEnabled? Proxy { get; set; }

    #endregion ProxyEnabled

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _reportErrors;
    partial void OnReportErrorsChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _timerEnabled;
    partial void OnTimerEnabledChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _topMostDisabled;
    partial void OnTopMostDisabledChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _useZipFile;
    partial void OnUseZipFileChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public Mode _updateMode;
    partial void OnUpdateModeChanged(Mode value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public RemindLaterFormat _timerDurationTimeSpan;
    partial void OnTimerDurationTimeSpanChanged(RemindLaterFormat value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public ushort _interval = 1;
    partial void OnIntervalChanged(ushort value) => OnUpdateValidation();

    #region UserSelectRemindLater

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _userSelectRemindLater;
    partial void OnUserSelectRemindLaterChanged(bool value) => OnUpdateValidation();

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public ushort _remindLaterAt = 1;
    partial void OnRemindLaterAtChanged(ushort value) => OnUpdateValidation();

    [ObservableProperty]
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RemindLaterFormat _remindLaterTimeSpan;
    partial void OnRemindLaterTimeSpanChanged(RemindLaterFormat value) => OnUpdateValidation();

    #endregion UserSelectRemindLater

    #region Version

    [ObservableProperty]
    [property: JsonIgnore]
    public bool _installedVersionOverride;
    partial void OnInstalledVersionOverrideChanging(bool value) => InstalledVersion = value ? new InstalledVersion { Version = _defaultInstalledVersion } : null;

    [ObservableProperty]
    [property: JsonIgnore]
    public ushort _majorVersion = (ushort)typeof(ViewModelMainConfig).Assembly.Version()!.Major;
    partial void OnMajorVersionChanged(ushort value)
    {
        var tmpVer = new Version(value, MinorVersion, BuildVersion, RevisionVersion);
        SetInstalledVersion(tmpVer);
    }

    [ObservableProperty]
    [property: JsonIgnore]
    public ushort _minorVersion = (ushort)typeof(ViewModelMainConfig).Assembly.Version()!.Minor;
    partial void OnMinorVersionChanged(ushort value)
    {
        var tmpVer = new Version(MajorVersion, value, BuildVersion, RevisionVersion);
        SetInstalledVersion(tmpVer);
    }

    [ObservableProperty]
    [property: JsonIgnore]
    public ushort _buildVersion = (ushort)typeof(ViewModelMainConfig).Assembly.Version()!.Build;
    partial void OnBuildVersionChanged(ushort value)
    {
        var tmpVer = new Version(MajorVersion, MinorVersion, value, RevisionVersion);
        SetInstalledVersion(tmpVer);
    }

    [ObservableProperty]
    [property: JsonIgnore]
    public ushort _revisionVersion = (ushort)typeof(ViewModelMainConfig).Assembly.Version()!.Revision;
    partial void OnRevisionVersionChanged(ushort value)
    {
        var tmpVer = new Version(MajorVersion, MinorVersion, BuildVersion, value);
        SetInstalledVersion(tmpVer);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InstalledVersion? InstalledVersion { get; set; }

    #endregion Version

    [ObservableProperty]
    [property: JsonIgnore]
    public ImageSource? _tmpIcon = null;
    partial void OnTmpIconChanged(ImageSource? value) => OnUpdateValidation();

    [ObservableProperty]
    [property: JsonIgnore]
    public ObservableCollection<TreeViewItem> _timerNodeList = [];

    [ObservableProperty]
    [property: JsonIgnore]
    public ObservableCollection<TreeViewItem> _applicationExitNodeList = [];

    [ObservableProperty]
    [property: JsonIgnore]
    public ObservableCollection<TreeViewItem> _checkForUpdatesNodeList = [];

    [ObservableProperty]
    [property: JsonIgnore]
    public ObservableCollection<TreeViewItem> _parseUpdateInfoNodeList = [];

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties

    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public event Action<bool?>? UpdateValidation;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    #region Event Invocators
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public void OnUpdateValidation(bool? isEnabled = null)
    {
        isEnabled ??= !Equals();
        UpdateValidation?.Invoke(isEnabled);
    }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Event Invocators


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public ViewModelMainConfig()
    {
    }


    /// <summary>
    ///     Copy Constructor
    /// </summary>
    /// <param name="other"></param>
    public ViewModelMainConfig(ViewModelMainConfig? other) : this()
    {
        Copy(other);
    }


    private void SetInstalledVersion(Version tmpVer)
    {
        if (_defaultInstalledVersion == tmpVer)
            InstalledVersion = null;
        else
        {
            if (InstalledVersion is not null)
                InstalledVersion.Version = tmpVer;
        }
    }


    protected internal ViewModelMainConfig? Copy(ViewModelMainConfig? other)
    {
        if (other is null)
            return null;

        AppTitle                      = other.AppTitle;
        BasicAuth                     = other.BasicAuth;
        BasicAuthChangeLog            = other.BasicAuthChangeLog;
        BasicAuthDownload             = other.BasicAuthDownload;
        BasicAuthPassword             = other.BasicAuthPassword;
        BasicAuthUserName             = other.BasicAuthUserName;
        ChangeUpdateZipExtractionPath = other.ChangeUpdateZipExtractionPath;
        CheckSynchronously            = other.CheckSynchronously;
        ClearAppDirectory             = other.ClearAppDirectory;
        DoNotBindOwnerWindow          = other.DoNotBindOwnerWindow;
        ExecutablePath                = other.ExecutablePath;
        ExecutablePathOverride        = other.ExecutablePathOverride;
        FtpProtocol                   = other.FtpProtocol;
        IconOverride                  = other.IconOverride;
        ImageUri                      = other.ImageUri;
        InstallationPath              = other.InstallationPath;
        InstalledVersion              = other.InstalledVersion;
        InstalledVersionOverride      = other.InstalledVersionOverride;
        Interval                      = other.Interval;
        IsManditory                   = other.IsManditory;
        OpenDownloadPage              = other.OpenDownloadPage;
        PersistSettings               = other.PersistSettings;
        ProxyEnabled                  = other.ProxyEnabled;
        MajorVersion                  = other.MajorVersion;
        MinorVersion                  = other.MinorVersion;
        BuildVersion                  = other.BuildVersion;
        RevisionVersion               = other.RevisionVersion;
        ProxyUri                      = other.ProxyUri;
        ProxyUserName                 = other.ProxyUserName;
        ProxyPassword                 = other.ProxyPassword;
        RemindLaterAt                 = other.RemindLaterAt;
        RemindLaterTimeSpan           = other.RemindLaterTimeSpan;
        ReportErrors                  = other.ReportErrors;
        RunUpdateAsAdmin              = other.RunUpdateAsAdmin;
        ShowRemindLaterButton         = other.ShowRemindLaterButton;
        ShowSkipButton                = other.ShowSkipButton;
        TimerDurationTimeSpan         = other.TimerDurationTimeSpan;
        TopMostDisabled               = other.TopMostDisabled;
        UpdateMode                    = other.UpdateMode;
        UserSelectRemindLater         = other.UserSelectRemindLater;
        UseZipFile                    = other.UseZipFile;

        return this;
    }

    public virtual bool Equals() => throw new NotImplementedException(nameof(Equals));

    public bool Equals(ViewModelMainConfig? other)
    {
        if (other == null)
            return false;

        return (
                   ReferenceEquals(AppTitle, other.AppTitle) ||
                   AppTitle == other.AppTitle
               ) &&
               (
                   ReferenceEquals(BasicAuthPassword, other.BasicAuthPassword) ||
                   BasicAuthPassword == other.BasicAuthPassword
               ) &&
               (
                   ReferenceEquals(BasicAuthUserName, other.BasicAuthUserName) ||
                   BasicAuthUserName == other.BasicAuthUserName
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
                   ReferenceEquals(Proxy?.Uri, other.Proxy?.Uri) ||
                   Proxy?.Uri == other.Proxy?.Uri
               ) &&
               (
                   ReferenceEquals(Proxy?.Password, other.Proxy?.Password) ||
                   Proxy?.Password == other.Proxy?.Password
               ) &&
               (
                   ReferenceEquals(Proxy?.UserName, other.Proxy?.UserName) ||
                   Proxy?.UserName == other.Proxy?.UserName
               )                                                                            &&
               BasicAuth                         == other.BasicAuth                         &&
               BasicAuthChangeLog                == other.BasicAuthChangeLog                &&
               BasicAuthDownload                 == other.BasicAuthDownload                 &&
               ChangeUpdateZipExtractionPath     == other.ChangeUpdateZipExtractionPath     &&
               CheckSynchronously                == other.CheckSynchronously                &&
               ClearAppDirectory                 == other.ClearAppDirectory                 &&
               DoNotBindOwnerWindow              == other.DoNotBindOwnerWindow              &&
               ExecutablePathOverride            == other.ExecutablePathOverride            &&
               FtpProtocol                       == other.FtpProtocol                       &&
               IconOverride                      == other.IconOverride                      &&
               Interval                          == other.Interval                          &&
               IsManditory                       == other.IsManditory                       &&
               BuildVersion                      == other.BuildVersion                      &&
               MajorVersion                      == other.MajorVersion                      &&
               MinorVersion                      == other.MinorVersion                      &&
               RevisionVersion                   == other.RevisionVersion                   &&
               OpenDownloadPage                  == other.OpenDownloadPage                  &&
               PersistSettings                   == other.PersistSettings                   &&
               ProxyEnabled                      == other.ProxyEnabled                      &&
               RemindLaterAt                     == other.RemindLaterAt                     &&
               RemindLaterTimeSpan               == other.RemindLaterTimeSpan               &&
               ReportErrors                      == other.ReportErrors                      &&
               RunUpdateAsAdmin                  == other.RunUpdateAsAdmin                  &&
               ShowRemindLaterButton             == other.ShowRemindLaterButton             &&
               ShowSkipButton                    == other.ShowSkipButton                    &&
               TimerDurationTimeSpan             == other.TimerDurationTimeSpan             &&
               TopMostDisabled                   == other.TopMostDisabled                   &&
               UpdateMode                        == other.UpdateMode                        &&
               UseZipFile                        == other.UseZipFile;
    }
}