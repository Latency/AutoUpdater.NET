// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelMain.Config.cs
// Author:   Latency McLaughlin
// Date:     06/18/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming
#pragma warning disable MVVMTK0042
#pragma warning disable CS0657 // Not a valid attribute location for this declaration

using AssemblyLoader;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AutoUpdaterDotNET.Extensions;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelMainConfig : ObservableObject, IViewModelMainConfig
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected Version _defaultInstalledVersion;

    private string? _defaultAppTitle;

    private readonly bool[] _defaultManditory = new bool[2];

    private   ProxyEnabled? _defaultProxy;

    private TimerEnabled? _defaultTimer, _defaultRemindLaterTimer;

    private IsManditory? _defaultIsManditory;

    private IconOverride? _defaultIconOverride;

    private BasicAuth? _defaultBasicAuth;

    private ZipFile? _defaultZipFile;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #region AppTitle
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _isAppTitle;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnIsAppTitleChanged(bool value)
    {
        _appTitle = value ? _defaultAppTitle : null;
        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public string? _appTitle;
    partial void OnAppTitleChanged(string? value)
    {
        _defaultAppTitle = value;
        UpdateValidation?.Invoke(!Equals());
    }
    #endregion AppTitle

    #region IsManditory
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _isManditory;

    partial void OnIsManditoryChanged(bool value)
    {
        Manditory = value ? _defaultIsManditory ??= new IsManditory() : null;

        if (Manditory is null)
        {
            _showSkipButton        = _defaultManditory[0];
            _showRemindLaterButton = _defaultManditory[1];
        }
        else
        {
            _showSkipButton        = false;
            _showRemindLaterButton = false;
        }

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _showSkipButton;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnShowSkipButtonChanged(bool value)
    {
        _defaultManditory[0] = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _showRemindLaterButton;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnShowRemindLaterButtonChanged(bool value)
    {
        _defaultManditory[1] = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public Mode _updateMode;
    partial void OnUpdateModeChanged(Mode value)
    {
        Manditory?.UpdateMode = value;

        if (_defaultIsManditory == Manditory)
            _defaultIsManditory = null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsManditory")]
    public IsManditory? Manditory { get; set; }
    #endregion IsManditory

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _runUpdateAsAdmin;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnRunUpdateAsAdminChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _openDownloadPage;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnOpenDownloadPageChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    #region Basic Auth

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BasicAuthentication")]
    public BasicAuth? BasicAuth { get; set; }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _isBasicAuth;
    partial void OnIsBasicAuthChanged(bool value)
    {
        BasicAuth = value && (_basicAuthChangeLog || _basicAuthDownload || !string.IsNullOrEmpty(_basicAuthUserName) || !string.IsNullOrEmpty(_basicAuthPassword)) ? _defaultBasicAuth ??= new BasicAuth() : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _basicAuthChangeLog;
    partial void OnBasicAuthChangeLogChanged(bool value)
    {
        BasicAuth            = value || _basicAuthDownload || !string.IsNullOrEmpty(_basicAuthUserName) || !string.IsNullOrEmpty(_basicAuthPassword) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.ChangeLog = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _basicAuthDownload;
    partial void OnBasicAuthDownloadChanged(bool value)
    {
        BasicAuth           = _basicAuthChangeLog || value || !string.IsNullOrEmpty(_basicAuthUserName) || !string.IsNullOrEmpty(_basicAuthPassword) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.Download = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _basicAuthUserName;
    partial void OnBasicAuthUserNameChanged(string? value)
    {
        BasicAuth           = _basicAuthChangeLog || _basicAuthDownload || !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(_basicAuthPassword) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.UserName = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _basicAuthPassword;
    partial void OnBasicAuthPasswordChanged(string? value)
    {
        BasicAuth           = _basicAuthChangeLog || _basicAuthDownload || !string.IsNullOrEmpty(_basicAuthUserName) || !string.IsNullOrEmpty(value) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.Password = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }
    #endregion Basic Auth

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _doNotBindOwnerWindow;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnDoNotBindOwnerWindowChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _checkSynchronously;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnCheckSynchronouslyChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _ftpProtocol;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnFtpProtocolChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    #region Icon Override
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _IsIconOverride;
    partial void OnIsIconOverrideChanged(bool value)
    {
        var uri = TmpIcon?.ToString();
        IconOverride = value ? _defaultIconOverride ??= new IconOverride { Uri = uri is null ? null : new Uri(uri) } : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IconOverride? IconOverride { get; set; }

    #endregion Icon Override

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _persistSettings;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnPersistSettingsChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    #region ProxyEnabled
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _proxyEnabled;
    partial void OnProxyEnabledChanged(bool value)
    {
        Proxy = value && (!string.IsNullOrEmpty(_proxyUri) || !string.IsNullOrEmpty(_proxyUserName) || !string.IsNullOrEmpty(_proxyPassword)) ? _defaultProxy ??= new ProxyEnabled() : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyUri;
    partial void OnProxyUriChanged(string? value)
    {
        Proxy      = !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(_proxyUserName) || !string.IsNullOrEmpty(_proxyPassword) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.Uri = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyUserName;
    partial void OnProxyUserNameChanged(string? value)
    {
        Proxy           = !string.IsNullOrEmpty(_proxyUri) || !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(_proxyPassword) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.UserName = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _proxyPassword;
    partial void OnProxyPasswordChanged(string? value)
    {
        Proxy           = !string.IsNullOrEmpty(_proxyUri) || !string.IsNullOrEmpty(_proxyUserName) || !string.IsNullOrEmpty(value) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.Password = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProxyEnabled? Proxy { get; set; }
    #endregion ProxyEnabled

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _reportErrors;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnReportErrorsChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public bool _topMostDisabled;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTopMostDisabledChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    #region Use ZipFile
    [property: JsonIgnore]
    [ObservableProperty]
    public bool _useZipFile;
    partial void OnUseZipFileChanged(bool value)
    {
        ZipFile = !value ? null : new ZipFile(); // _clearAppDirectory || (_executablePathOverride && !string.IsNullOrEmpty(_executablePath)) || (_zipExtractionPathOverride && !string.IsNullOrEmpty(_installationPath)) ? _defaultZipFile ??= new ZipFile() : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _clearAppDirectory;
    partial void OnClearAppDirectoryChanged(bool value)
    {
        ZipFile                    = _clearAppDirectory || (_executablePathOverride && !string.IsNullOrEmpty(_executablePath)) || (_zipExtractionPathOverride && !string.IsNullOrEmpty(_installationPath)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ClearAppDirectory = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _executablePathOverride;
    partial void OnExecutablePathOverrideChanged(bool value)
    {
        ZipFile                               = _clearAppDirectory || (_executablePathOverride && !string.IsNullOrEmpty(_executablePath)) || (_zipExtractionPathOverride && !string.IsNullOrEmpty(_installationPath)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ExecutablePathOverride       = value && !string.IsNullOrEmpty(_executablePath) ? new FilePath() : null;
        ZipFile?.ExecutablePathOverride?.Path = _executablePath;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _executablePath;

    partial void OnExecutablePathChanged(string? value)
    {
        ZipFile                               = _clearAppDirectory || (_executablePathOverride && !string.IsNullOrEmpty(value)) || (_zipExtractionPathOverride && !string.IsNullOrEmpty(_installationPath)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ExecutablePathOverride       = !string.IsNullOrEmpty(value) ? new FilePath() : null;
        ZipFile?.ExecutablePathOverride?.Path = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public bool _zipExtractionPathOverride;
    partial void OnZipExtractionPathOverrideChanged(bool value)
    {
        ZipFile                                  = _clearAppDirectory || (_executablePathOverride && !string.IsNullOrEmpty(_executablePath)) || (_zipExtractionPathOverride && !string.IsNullOrEmpty(_installationPath)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ZipExtractionPathOverride       = value && !string.IsNullOrEmpty(_installationPath) ? new FilePath() : null;
        ZipFile?.ZipExtractionPathOverride?.Path = _installationPath;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public string? _installationPath;
    partial void OnInstallationPathChanged(string? value)
    {
        ZipFile                                  = _clearAppDirectory || (_executablePathOverride && !string.IsNullOrEmpty(_executablePath)) || (_zipExtractionPathOverride && !string.IsNullOrEmpty(value)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ZipExtractionPathOverride       = !string.IsNullOrEmpty(value) ? new FilePath() : null;
        ZipFile?.ZipExtractionPathOverride?.Path = value;

        UpdateValidation?.Invoke(!Equals());
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

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public ushort _timerInterval = 1;
    partial void OnTimerIntervalChanged(ushort value)
    {
        Timer?.Interval = value;

        if (_defaultTimer == Timer)
            _defaultTimer = null;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public RemindLaterFormat _timerDurationTimeSpan = RemindLaterFormat.Seconds;
    partial void OnTimerDurationTimeSpanChanged(RemindLaterFormat value)
    {
        Timer?.TimeSpan = value;

        if (_defaultTimer == Timer)
            _defaultTimer = null;

        UpdateValidation?.Invoke(!Equals());
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

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public ushort _remindLaterAt = 1;
    partial void OnRemindLaterAtChanged(ushort value)
    {
        RemmindLaterTimer?.Interval = value;

        if (_defaultRemindLaterTimer == Timer)
            _defaultRemindLaterTimer = null;

        UpdateValidation?.Invoke(!Equals());
    }

    [property: JsonIgnore]
    [ObservableProperty]
    public RemindLaterFormat _remindLaterTimeSpan = RemindLaterFormat.Seconds;
    partial void OnRemindLaterTimeSpanChanged(RemindLaterFormat value)
    {
        RemmindLaterTimer?.TimeSpan = value;

        if (_defaultRemindLaterTimer == Timer)
            _defaultRemindLaterTimer = null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UserSelectRemmindLater")]
    public TimerEnabled? RemmindLaterTimer { get; set; }

    #endregion UserSelectRemindLater

    #region Version

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InstalledVersion? InstalledVersion { get; set; }

    [ObservableProperty]
    [property: JsonIgnore]
    public bool _installedVersionOverride;
    partial void OnInstalledVersionOverrideChanged(bool value)
    {
        InstalledVersion = value ? new InstalledVersion { Version = new Version(MajorVersion, MinorVersion, BuildVersion, RevisionVersion) } : null;
        UpdateValidation?.Invoke(!Equals());
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

    #endregion Version

    [ObservableProperty]
    [property: JsonIgnore]
    public BitmapImage? _tmpIcon = null;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTmpIconChanged(BitmapImage? value) => UpdateValidation?.Invoke(!Equals());

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
    public event Action<bool?>?        UpdateValidation;
    public event Action<string?>?      UpdateVersion;
    public event Action<BitmapImage?>? UpdateIcon;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    #region Event Invocators
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected void _UpdateValidation(bool?  value = null) => UpdateValidation?.Invoke(value);
    protected void _UpdateIcon(BitmapImage? value)
    {
        if (!IsIconOverride)
        {
            var uri = _defaultIconOverride?.Uri;
            if (!string.IsNullOrEmpty(uri?.ToString()))
            {
                TmpIcon = uri.ConvertToBitmapImage();
                value   = TmpIcon;
            }
        }

        UpdateIcon?.Invoke(value);
    }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Event Invocators


    #region Constructors
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public ViewModelMainConfig()
    {
        TmpIcon              = Application.Current!.Resources["Project"] as BitmapImage;
        _defaultIconOverride = new IconOverride
        {
            Uri = string.IsNullOrEmpty(TmpIcon?.ToString()) ? null : new Uri(TmpIcon.ToString()!)
        };
        _defaultInstalledVersion = Assembly.GetExecutingAssembly().Version()!;
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


    public virtual void Update()
    {
        UpdateVersion?.Invoke($"Loader Version: {InstalledVersion?.Version ?? _defaultInstalledVersion}");
    }


    private void SetInstalledVersion(Version tmpVer)
    {
        if (_defaultInstalledVersion == tmpVer)
            InstalledVersion = null;
        else
        {
            InstalledVersion         ??= new InstalledVersion();
            InstalledVersion.Version   = tmpVer;
        }

        UpdateValidation?.Invoke(!Equals());
    }


    protected internal ViewModelMainConfig? Copy(ViewModelMainConfig? other)
    {
        if (other is null)
            return null;

        RunUpdateAsAdmin = other.RunUpdateAsAdmin;
        OpenDownloadPage = other.OpenDownloadPage;

        RemmindLaterTimer     = other.RemmindLaterTimer;                                        // User Select Remind Later
        UserSelectRemindLater = other.RemmindLaterTimer != null;                                // User Select Remind Later
        RemindLaterAt         = other.RemmindLaterTimer?.Interval ?? 1;                         // User Select Remind Later
        RemindLaterTimeSpan   = other.RemmindLaterTimer?.TimeSpan ?? RemindLaterFormat.Seconds; // User Select Remind Later

        Manditory             = other.Manditory;                            // IsManditory
        IsManditory           = other.Manditory != null;                    // IsManditory
        UpdateMode            = other.Manditory?.UpdateMode ?? Mode.Normal; // IsManditory (True) -> Update Mode
        ShowSkipButton        = other.ShowSkipButton;                       // IsManditory (False) -> Show Skip Button
        ShowRemindLaterButton = other.ShowRemindLaterButton;                // IsManditory (False) -> Show Remind Later Button

        AppTitle   = other.AppTitle;         // App Title -> Title
        IsAppTitle = other.AppTitle != null; // App Title

        ReportErrors = other.ReportErrors;

        Proxy         = other.Proxy;           // Enable Proxy
        ProxyEnabled  = other.Proxy != null;   // Enable Proxy
        ProxyUri      = other.Proxy?.Uri;      // Enable Proxy -> Uri
        ProxyUserName = other.Proxy?.UserName; // Enable Proxy -> UserName
        ProxyPassword = other.Proxy?.Password; // Enable Proxy -> Password

        Timer                 = other.Timer;                                        // Enable Timer
        TimerEnabled          = other.Timer != null;                                // Enable Timer
        TimerInterval         = other.Timer?.Interval ?? 1;                         // Enable Timer -> SpinBox
        TimerDurationTimeSpan = other.Timer?.TimeSpan ?? RemindLaterFormat.Seconds; // Enable Timer -> ComboBox

        BasicAuth          = other.BasicAuth;                     // Basic Authentication
        IsBasicAuth        = other.BasicAuth != null;             // Basic Authentication
        BasicAuthChangeLog = other.BasicAuth?.ChangeLog ?? false; // Basic Authentication -> Change Log
        BasicAuthDownload  = other.BasicAuth?.Download  ?? false; // Basic Authentication -> Download
        BasicAuthPassword  = other.BasicAuth?.Password;           // Basic Authentication -> Password
        BasicAuthUserName  = other.BasicAuth?.UserName;           // Basic Authentication -> UserName

        FtpProtocol          = other.FtpProtocol;
        PersistSettings      = other.PersistSettings;

        ZipFile                   = other.ZipFile;                                    // Use Zip File
        UseZipFile                = other.ZipFile != null;                            // Use Zip File
        ClearAppDirectory         = other.ZipFile?.ClearAppDirectory ?? false;        // Use Zip File -> Clear App Directory
        ExecutablePathOverride    = other.ZipFile?.ExecutablePathOverride != null;    // Use Zip File -> Executable Path Override
        ExecutablePath            = other.ZipFile?.ExecutablePathOverride?.Path;      // Use Zip File -> Executable Path Override
        ZipExtractionPathOverride = other.ZipFile?.ZipExtractionPathOverride != null; // Use Zip File -> Zip Extraction Path Override
        InstallationPath          = other.ZipFile?.ZipExtractionPathOverride?.Path;   // Use Zip File -> Zip Extraction Path Override

        CheckSynchronously = other.CheckSynchronously;

        InstalledVersion         = other.InstalledVersion;                                   // Installed Version Override
        InstalledVersionOverride = other.InstalledVersion != null;                           // Installed Version Override
        MajorVersion             = (ushort)(other.InstalledVersion?.Version?.Major    ?? 1); // Installed Version Override -> SpinBox [Major]
        MinorVersion             = (ushort)(other.InstalledVersion?.Version?.Minor    ?? 0); // Installed Version Override -> SpinBox [Minor]
        BuildVersion             = (ushort)(other.InstalledVersion?.Version?.Build    ?? 0); // Installed Version Override -> SpinBox [Build]
        RevisionVersion          = (ushort)(other.InstalledVersion?.Version?.Revision ?? 0); // Installed Version Override -> SpinBox [Revision]

        DoNotBindOwnerWindow = other.DoNotBindOwnerWindow;
        TopMostDisabled      = other.TopMostDisabled;

        IconOverride   = other.IconOverride;                                                               // Icon Override
        IsIconOverride = other.IconOverride != null;                                                       // Icon Override
        TmpIcon        = other.IconOverride?.Uri is null ? null : new BitmapImage(other.IconOverride.Uri); // Icon Override -> (ICO)

        return this;
    }


    public virtual bool Equals() => throw new NotImplementedException(nameof(Equals));


    public bool Equals(ViewModelMainConfig? other)
    {
        if (other == null)
            return false;

        return IsVersionOverride()                                      &&
               IsMandatory()                                            &&
               IsIconOverride()                                         &&
               AppTitle                  == other.AppTitle              &&
               BasicAuthPassword         == other.BasicAuthPassword     &&
               BasicAuthUserName         == other.BasicAuthUserName     &&
               ExecutablePath            == other.ExecutablePath        &&
               InstallationPath          == other.InstallationPath      &&
               Proxy?.Uri                == other.Proxy?.Uri            &&
               Proxy?.Password           == other.Proxy?.Password       &&
               Proxy?.UserName           == other.Proxy?.UserName       &&
               BasicAuth                 == other.BasicAuth             &&
               BasicAuthChangeLog        == other.BasicAuthChangeLog    &&
               BasicAuthDownload         == other.BasicAuthDownload     &&
               CheckSynchronously        == other.CheckSynchronously    &&
               ClearAppDirectory         == other.ClearAppDirectory     &&
               DoNotBindOwnerWindow      == other.DoNotBindOwnerWindow  &&
               FtpProtocol               == other.FtpProtocol           &&
               OpenDownloadPage          == other.OpenDownloadPage      &&
               PersistSettings           == other.PersistSettings       &&
               ProxyEnabled              == other.ProxyEnabled          &&
               RemindLaterAt             == other.RemindLaterAt         &&
               RemindLaterTimeSpan       == other.RemindLaterTimeSpan   &&
               ReportErrors              == other.ReportErrors          &&
               RunUpdateAsAdmin          == other.RunUpdateAsAdmin      &&
               TimerDurationTimeSpan     == other.TimerDurationTimeSpan &&
               TimerInterval             == other.TimerInterval         &&
               TopMostDisabled           == other.TopMostDisabled       &&
               UserSelectRemindLater     == other.UserSelectRemindLater &&
               ZipExtractionPathOverride == other.ZipExtractionPathOverride;


        bool IsIconOverride()
        {
            if (other.IsIconOverride)
            {
                return TmpIcon == other.TmpIcon;
            }

            return this.IsIconOverride == other.IsIconOverride;
        }

        bool IsVersionOverride()
        {
            if (other.InstalledVersionOverride)
            {
                return
                    MajorVersion    == other.MajorVersion &&
                    MinorVersion    == other.MinorVersion &&
                    BuildVersion    == other.BuildVersion &&
                    RevisionVersion == other.RevisionVersion;
            }

            return InstalledVersionOverride == other.InstalledVersionOverride;
        }

        bool IsMandatory()
        {
            if (!other.IsManditory)
                return ShowRemindLaterButton == other.ShowRemindLaterButton &&
                       ShowSkipButton        == other.ShowSkipButton        &&
                       IsManditory           == other.IsManditory;

            return UpdateMode  == other.UpdateMode &&
                   IsManditory == other.IsManditory;
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