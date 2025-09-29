// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     06/18/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming
#pragma warning disable MVVMTK0042
#pragma warning disable CS0657 // Not a valid attribute location for this declaration

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
    protected Version _defaultInstalledVersion = null!;

    private ProxyEnabled? _defaultProxy;

    private TimerEnabled? _defaultTimer, _defaultRemindLaterTimer;

    private IsManditory? _defaultIsManditory;

    private IconOverride? _defaultIconOverride;

    private BasicAuth? _defaultBasicAuth;

    private ZipFile? _defaultZipFile;

    private FilePath? _defaultZipExtractionPathOverride, _defaultExecutablePathOverride;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #region AppTitle
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _isAppTitle;
    partial void OnIsAppTitleChanged(bool value)
    {
        _isAppTitle = value;
        OnUpdateValidation();
    }

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _appTitle;
    partial void OnAppTitleChanged(string? value)
    {
        AppTitle = !string.IsNullOrEmpty(value) ? value : null;
        OnUpdateValidation();
    }
    #endregion AppTitle

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _executablePath;
    partial void OnExecutablePathChanged(string? value)
    {
        ZipFile?.ExecutablePathOverride?.Path = value;
        OnUpdateValidation();
    }

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _imageUri;
    partial void OnImageUriChanged(string? value) => OnUpdateValidation();

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _installationPath;
    partial void OnInstallationPathChanged(string? value)
    {
        ZipFile?.ZipExtractionPathOverride?.Path = value;
        OnUpdateValidation();
    }

    #region IsManditory
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _isManditory;
    partial void OnIsManditoryChanged(bool value)
    {
        Manditory = value ? _defaultIsManditory ??= new IsManditory() : null;
        OnUpdateValidation();
    }

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _showSkipButton;
    partial void OnShowSkipButtonChanged(bool value) => OnUpdateValidation(value);

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _showRemindLaterButton;
    partial void OnShowRemindLaterButtonChanged(bool value) => OnUpdateValidation(value);

    [property: JsonIgnore]
    [ObservableProperty]
    public Mode _updateMode;
    partial void OnUpdateModeChanged(Mode value)
    {
        Manditory?.UpdateMode = value;

        if (_defaultIsManditory == Manditory)
            _defaultIsManditory = null;

        OnUpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsManditory")]
    public IsManditory? Manditory { get; set; }
    #endregion IsManditory

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _runUpdateAsAdmin;
    partial void OnRunUpdateAsAdminChanged(bool value) => OnUpdateValidation(value);

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _openDownloadPage;
    partial void OnOpenDownloadPageChanged(bool value) => OnUpdateValidation(value);

    #region Basic Auth
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _isBasicAuth;
    partial void OnIsBasicAuthChanged(bool value)
    {
        BasicAuth = value ? _defaultBasicAuth ??= new BasicAuth() : null;
        OnUpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BasicAuthentication")]
    public BasicAuth? BasicAuth { get; set; }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _basicAuthChangeLog;
    partial void OnBasicAuthChangeLogChanged(bool value)
    {
        BasicAuth?.ChangeLog = value;
        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _basicAuthDownload;
    partial void OnBasicAuthDownloadChanged(bool value)
    {
        BasicAuth?.Download = value;
        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _basicAuthUserName;
    partial void OnBasicAuthUserNameChanged(string? value)
    {
        BasicAuth?.UserName = !string.IsNullOrEmpty(value) ? value : null;
        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _basicAuthPassword;
    partial void OnBasicAuthPasswordChanged(string? value)
    {
        BasicAuth?.Password = !string.IsNullOrEmpty(value) ? value : null;
        OnUpdateValidation();
    }
    #endregion Basic Auth

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _doNotBindOwnerWindow;
    partial void OnDoNotBindOwnerWindowChanged(bool value) => OnUpdateValidation(value);

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _checkSynchronously;
    partial void OnCheckSynchronouslyChanged(bool value) => OnUpdateValidation(value);

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _ftpProtocol;
    partial void OnFtpProtocolChanged(bool value) => OnUpdateValidation(value);

    #region Icon Override
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _IsIconOverride;
    partial void OnIsIconOverrideChanged(bool value)
    {
        IconOverride = value ? _defaultIconOverride ??= new IconOverride { Uri = TmpIcon?.ToString() } : null;
        OnUpdateValidation(value);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IconOverride? IconOverride { get; set; }

    #endregion Icon Override

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _persistSettings;
    partial void OnPersistSettingsChanged(bool value) => OnUpdateValidation(value);

    #region ProxyEnabled
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _proxyEnabled;
    partial void OnProxyEnabledChanged(bool value)
    {
        Proxy = value ? _defaultProxy ??= new ProxyEnabled() : null;
        OnUpdateValidation(value);
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyUri;
    partial void OnProxyUriChanged(string? value)
    {
        Proxy?.Uri = !string.IsNullOrEmpty(value) ? value : null;
        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyUserName;
    partial void OnProxyUserNameChanged(string? value)
    {
        Proxy?.UserName = !string.IsNullOrEmpty(value) ? value : null;
        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyPassword;
    partial void OnProxyPasswordChanged(string? value)
    {
        Proxy?.Password = !string.IsNullOrEmpty(value) ? value : null;
        OnUpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProxyEnabled? Proxy { get; set; }
    #endregion ProxyEnabled

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _reportErrors;
    partial void OnReportErrorsChanged(bool value) => OnUpdateValidation(value);

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _topMostDisabled;
    partial void OnTopMostDisabledChanged(bool value) => OnUpdateValidation(value);

    #region Use ZipFile
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _useZipFile;
    partial void OnUseZipFileChanged(bool value)
    {
        ZipFile = value ? _defaultZipFile ??= new ZipFile() : null;
        OnUpdateValidation(value);
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _clearAppDirectory;
    partial void OnClearAppDirectoryChanged(bool value)
    {
        ZipFile?.ClearAppDirectory = value;
        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _executablePathOverride;
    partial void OnExecutablePathOverrideChanged(bool value)
    {
        ZipFile!.ExecutablePathOverride = value ? _defaultExecutablePathOverride ??= new FilePath() : null;
        OnUpdateValidation(value);
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _zipExtractionPathOverride;
    partial void OnZipExtractionPathOverrideChanged(bool value)
    {
        ZipFile!.ZipExtractionPathOverride = value ? _defaultZipExtractionPathOverride ??= new FilePath() : null;
        OnUpdateValidation(value);
    }
    #endregion Use ZipFile

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ZipFile? ZipFile { get; set; }
    #region TimerEnabled

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _timerEnabled;
    partial void OnTimerEnabledChanged(bool value)
    {
        Timer = value ? _defaultTimer ??= new TimerEnabled() : null;
        OnUpdateValidation(value);
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public ushort _timerInterval = 1;
    partial void OnTimerIntervalChanged(ushort value)
    {
        Timer?.Interval = value;

        if (_defaultTimer == Timer)
            _defaultTimer = null;

        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public RemindLaterFormat _timerDurationTimeSpan;
    partial void OnTimerDurationTimeSpanChanged(RemindLaterFormat value)
    {
        Timer?.TimeSpan = value;

        if (_defaultTimer == Timer)
            _defaultTimer = null;

        OnUpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TimerEnabled? Timer { get; set; }
    #endregion TimerEnabled

    #region UserSelectRemindLater

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _userSelectRemindLater;
    partial void OnUserSelectRemindLaterChanged(bool value)
    {
        RemmindLaterTimer = value ? _defaultRemindLaterTimer ??= new TimerEnabled() : null;
        OnUpdateValidation(value);
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public ushort _remindLaterAt = 1;
    partial void OnRemindLaterAtChanged(ushort value)
    {
        RemmindLaterTimer?.Interval = value;

        if (_defaultRemindLaterTimer == Timer)
            _defaultRemindLaterTimer = null;

        OnUpdateValidation();
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public RemindLaterFormat _remindLaterTimeSpan;
    partial void OnRemindLaterTimeSpanChanged(RemindLaterFormat value)
    {
        RemmindLaterTimer?.TimeSpan = value;

        if (_defaultRemindLaterTimer == Timer)
            _defaultRemindLaterTimer = null;

        OnUpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UserSelectRemmindLater")]
    public TimerEnabled? RemmindLaterTimer { get; set; }

    #endregion UserSelectRemindLater

    #region Version

    [ObservableProperty]
    [property: JsonIgnore]
    public bool _installedVersionOverride;
    partial void OnInstalledVersionOverrideChanging(bool value)
    {
        InstalledVersion = value ? new InstalledVersion { Version = _defaultInstalledVersion } : null;
        OnUpdateValidation(value);
    }

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
    partial void OnTmpIconChanged(ImageSource? value) => OnUpdateValidation(value is not null);

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


    #region Constructors
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
    #endregion Constructors


    private void SetInstalledVersion(Version tmpVer)
    {
        if (_defaultInstalledVersion == tmpVer)
            InstalledVersion = null;
        else
            InstalledVersion?.Version = tmpVer;

        OnUpdateValidation();
    }


    protected internal ViewModelMainConfig? Copy(ViewModelMainConfig? other)
    {
        if (other is null)
            return null;

        AppTitle                  = other.AppTitle;
        BasicAuth                 = other.BasicAuth;
        BasicAuthChangeLog        = other.BasicAuthChangeLog;
        BasicAuthDownload         = other.BasicAuthDownload;
        BasicAuthPassword         = other.BasicAuthPassword;
        BasicAuthUserName         = other.BasicAuthUserName;
        BuildVersion              = other.BuildVersion;
        CheckSynchronously        = other.CheckSynchronously;
        ClearAppDirectory         = other.ClearAppDirectory;
        DoNotBindOwnerWindow      = other.DoNotBindOwnerWindow;
        ExecutablePath            = other.ExecutablePath;
        ExecutablePathOverride    = other.ExecutablePathOverride;
        FtpProtocol               = other.FtpProtocol;
        IconOverride              = other.IconOverride;
        ImageUri                  = other.ImageUri;
        InstallationPath          = other.InstallationPath;
        InstalledVersion          = other.InstalledVersion;
        InstalledVersionOverride  = other.InstalledVersionOverride;
        IsAppTitle                = other.IsAppTitle;
        IsBasicAuth               = other.IsBasicAuth;
        IsIconOverride            = other.IsIconOverride;
        IsManditory               = other.IsManditory;
        MajorVersion              = other.MajorVersion;
        MinorVersion              = other.MinorVersion;
        OpenDownloadPage          = other.OpenDownloadPage;
        PersistSettings           = other.PersistSettings;
        ProxyEnabled              = other.ProxyEnabled;
        ProxyPassword             = other.ProxyPassword;
        ProxyUri                  = other.ProxyUri;
        ProxyUserName             = other.ProxyUserName;
        RemindLaterAt             = other.RemindLaterAt;
        RemindLaterTimeSpan       = other.RemindLaterTimeSpan;
        ReportErrors              = other.ReportErrors;
        RevisionVersion           = other.RevisionVersion;
        RunUpdateAsAdmin          = other.RunUpdateAsAdmin;
        ShowRemindLaterButton     = other.ShowRemindLaterButton;
        ShowSkipButton            = other.ShowSkipButton;
        TimerDurationTimeSpan     = other.TimerDurationTimeSpan;
        TimerEnabled              = other.TimerEnabled;
        TimerInterval             = other.TimerInterval;
        TmpIcon                   = other.TmpIcon;
        TopMostDisabled           = other.TopMostDisabled;
        UpdateMode                = other.UpdateMode;
        UserSelectRemindLater     = other.UserSelectRemindLater;
        UseZipFile                = other.UseZipFile;
        ZipExtractionPathOverride = other.ZipExtractionPathOverride;

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
               )                                                            &&
               IsMandatory()                                                &&
               BasicAuth                 == other.BasicAuth                 &&
               BasicAuthChangeLog        == other.BasicAuthChangeLog        &&
               BasicAuthDownload         == other.BasicAuthDownload         &&
               BuildVersion              == other.BuildVersion              &&
               CheckSynchronously        == other.CheckSynchronously        &&
               ClearAppDirectory         == other.ClearAppDirectory         &&
               DoNotBindOwnerWindow      == other.DoNotBindOwnerWindow      &&
               FtpProtocol               == other.FtpProtocol               &&
               MajorVersion              == other.MajorVersion              &&
               MinorVersion              == other.MinorVersion              &&
               OpenDownloadPage          == other.OpenDownloadPage          &&
               PersistSettings           == other.PersistSettings           &&
               ProxyEnabled              == other.ProxyEnabled              &&
               RemindLaterAt             == other.RemindLaterAt             &&
               RemindLaterTimeSpan       == other.RemindLaterTimeSpan       &&
               ReportErrors              == other.ReportErrors              &&
               RevisionVersion           == other.RevisionVersion           &&
               RunUpdateAsAdmin          == other.RunUpdateAsAdmin          &&
               TimerDurationTimeSpan     == other.TimerDurationTimeSpan     &&
               TimerInterval             == other.TimerInterval             &&
               TopMostDisabled           == other.TopMostDisabled           &&
               UserSelectRemindLater     == other.UserSelectRemindLater     &&
               ZipExtractionPathOverride == other.ZipExtractionPathOverride;


        bool IsMandatory()
        {
            if (IsManditory == other.IsManditory)
                return ShowRemindLaterButton == other.ShowRemindLaterButton &&
                       ShowSkipButton        == other.ShowSkipButton;

            return UpdateMode == other.UpdateMode;
        }
    }


    public override bool Equals(object? obj) => Equals(obj as ViewModelMainConfig);

    public override int GetHashCode()
    {
        // Combine hash codes of relevant fields
        unchecked // Allows arithmetic overflow without throwing an exception
        {
            var hash = 17;                                                              // A prime number
            hash = hash * 3 + (AppTitle          != null ? AppTitle.GetHashCode() : 0); // Another prime number
            hash = hash * 3 + (BasicAuthPassword != null ? BasicAuthPassword.GetHashCode() : 0);
            hash = hash * 3 + (BasicAuthUserName != null ? BasicAuthUserName.GetHashCode() : 0);
            hash = hash * 3 + (ExecutablePath    != null ? ExecutablePath.GetHashCode() : 0);
            hash = hash * 3 + (ImageUri          != null ? ImageUri.GetHashCode() : 0);
            hash = hash * 3 + (InstallationPath  != null ? InstallationPath.GetHashCode() : 0);

            // ReSharper disable NonReadonlyMemberInGetHashCode
            if (Proxy is not null)
            {
                hash = hash * 3 + (Proxy.Uri      != null ? Proxy.Uri.GetHashCode() : 0);
                hash = hash * 3 + (Proxy.Password != null ? Proxy.Password.GetHashCode() : 0);
                hash = hash * 3 + (Proxy.UserName != null ? Proxy.UserName.GetHashCode() : 0);
            }
            // ReSharper restore NonReadonlyMemberInGetHashCode

            if (IsManditory)
            {
                hash = hash * 3 + ShowRemindLaterButton.GetHashCode();
                hash = hash * 3 + ShowSkipButton.GetHashCode();
            }
            else
            {
                hash = hash * 3 + UpdateMode.GetHashCode();
            }

            // ReSharper disable NonReadonlyMemberInGetHashCode
            hash = hash * 3 + (BasicAuth != null ? BasicAuth.GetHashCode() : 0);
            // ReSharper restore NonReadonlyMemberInGetHashCode

            hash = hash * 3 + BasicAuthChangeLog.GetHashCode();
            hash = hash * 3 + BasicAuthDownload.GetHashCode();
            hash = hash * 3 + ZipExtractionPathOverride.GetHashCode();
            hash = hash * 3 + CheckSynchronously.GetHashCode();
            hash = hash * 3 + ClearAppDirectory.GetHashCode();
            hash = hash * 3 + DoNotBindOwnerWindow.GetHashCode();
            hash = hash * 3 + ExecutablePathOverride.GetHashCode();
            hash = hash * 3 + FtpProtocol.GetHashCode();
            hash = hash * 3 + TimerInterval.GetHashCode();
            hash = hash * 3 + BuildVersion.GetHashCode();
            hash = hash * 3 + MajorVersion.GetHashCode();
            hash = hash * 3 + MinorVersion.GetHashCode();
            hash = hash * 3 + RevisionVersion.GetHashCode();
            hash = hash * 3 + OpenDownloadPage.GetHashCode();
            hash = hash * 3 + PersistSettings.GetHashCode();
            hash = hash * 3 + ProxyEnabled.GetHashCode();
            hash = hash * 3 + RemindLaterAt.GetHashCode();
            hash = hash * 3 + RemindLaterTimeSpan.GetHashCode();
            hash = hash * 3 + ReportErrors.GetHashCode();
            hash = hash * 3 + RunUpdateAsAdmin.GetHashCode();
            hash = hash * 3 + TimerDurationTimeSpan.GetHashCode();
            hash = hash * 3 + TopMostDisabled.GetHashCode();
            hash = hash * 3 + (TmpIcon != null ? TmpIcon.GetHashCode() : 0);
            hash = hash * 3 + UserSelectRemindLater.GetHashCode();

            return hash;
        }
    }
}