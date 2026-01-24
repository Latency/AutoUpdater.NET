// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.Main.cs
// Author:   Latency McLaughlin
// Date:     06/18/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming
#pragma warning disable CS0657 // Not a valid attribute location for this declaration

using AssemblyLoader;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelMainConfig : ObservableObject, IViewModelMainConfig
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected IServiceProvider _serviceProvider;

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
    /// <summary>
    ///     Set the Application Title shown in Update dialog. Although AutoUpdater.NET will get it automatically, you can set
    ///     this property if you like to give custom Title.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial bool IsAppTitle { get; set; }
    partial void OnIsAppTitleChanged(bool value)
    {
        AppTitle = value ? _defaultAppTitle : null;
        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial string? AppTitle { get; set; }
    partial void OnAppTitleChanged(string? value)
    {
        _defaultAppTitle = value;
        UpdateValidation?.Invoke(!Equals());
    }
    #endregion AppTitle

    #region IsManditory
    [JsonIgnore]
    [ObservableProperty]
    public partial bool IsManditory { get; set; }
    partial void OnIsManditoryChanged(bool value)
    {
        Manditory = value ? _defaultIsManditory ??= new IsManditory() : null;

        if (Manditory is null)
        {
            ShowSkipButton = _defaultManditory[0];
            ShowRemindLaterButton = _defaultManditory[1];

            if (!(ShowSkipButton | ShowRemindLaterButton))
                ShowSkipButton = true;
        }
        else
        {
            ShowSkipButton = false;
            ShowRemindLaterButton = false;
        }

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     If this is true users can see the skip button.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool ShowSkipButton { get; set; } = true;
    partial void OnShowSkipButtonChanged(bool value)
    {
        _defaultManditory[0] = value;

        if (!(value | ShowRemindLaterButton))
            IsManditory = true;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     If this is true users can see the Remind Later button.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool ShowRemindLaterButton { get; set; } = true;
    partial void OnShowRemindLaterButtonChanged(bool value)
    {
        _defaultManditory[1] = value;

        if (!(value | ShowSkipButton))
            IsManditory = true;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     Set this to any of the available modes to change behaviour of the Mandatory flag.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial Mode UpdateMode { get; set; } = Mode.Normal;
    partial void OnUpdateModeChanged(Mode value)
    {
        Manditory?.UpdateMode = value;

        if (_defaultIsManditory == Manditory)
            _defaultIsManditory = null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial IsManditory? Manditory { get; set; }
    #endregion IsManditory

    /// <summary>
    ///     Set this to false if your application doesn't need administrator privileges to replace the old version.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool RunUpdateAsAdmin { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnRunUpdateAsAdminChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    /// <summary>
    ///     Opens the download URL in default browser if true. Very useful if you have portable application.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool OpenDownloadPage { get; set; } = true;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnOpenDownloadPageChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    #region Basic Auth

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BasicAuthentication")]
    [ObservableProperty]
    public partial BasicAuth? BasicAuth { get; set; }

    [JsonIgnore]
    [ObservableProperty]
    public partial bool IsBasicAuth { get; set; }
    partial void OnIsBasicAuthChanged(bool value)
    {
        BasicAuth = value && (BasicAuthChangeLog || BasicAuthDownload || !string.IsNullOrEmpty(BasicAuthUserName) || !string.IsNullOrEmpty(BasicAuthPassword)) ? _defaultBasicAuth ??= new BasicAuth() : null;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     Set Basic Authentication credentials to navigate to the change log URL.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial bool BasicAuthChangeLog { get; set; }  // System.Net.ICredentials?
    partial void OnBasicAuthChangeLogChanged(bool value)
    {
        BasicAuth            = value || BasicAuthDownload || !string.IsNullOrEmpty(BasicAuthUserName) || !string.IsNullOrEmpty(BasicAuthPassword) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.ChangeLog = value;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     Set Basic Authentication credentials required to download the file.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial bool BasicAuthDownload { get; set; }  // System.Net.ICredentials?
    partial void OnBasicAuthDownloadChanged(bool value)
    {
        BasicAuth           = BasicAuthChangeLog || value || !string.IsNullOrEmpty(BasicAuthUserName) || !string.IsNullOrEmpty(BasicAuthPassword) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.Download = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? BasicAuthUserName { get; set; }
    partial void OnBasicAuthUserNameChanged(string? value)
    {
        BasicAuth           = BasicAuthChangeLog || BasicAuthDownload || !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(BasicAuthPassword) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.UserName = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? BasicAuthPassword { get; set; }
    partial void OnBasicAuthPasswordChanged(string? value)
    {
        BasicAuth           = BasicAuthChangeLog || BasicAuthDownload || !string.IsNullOrEmpty(BasicAuthUserName) || !string.IsNullOrEmpty(value) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.Password = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }
    #endregion Basic Auth

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool DoNotBindOwnerWindow { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnDoNotBindOwnerWindowChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    /// <summary>
    ///     Set this to true if you want to run update check synchronously.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool CheckSynchronously { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnCheckSynchronouslyChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool FtpProtocol { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnFtpProtocolChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    #region Icon Override
    [JsonIgnore]
    [ObservableProperty]
    public partial bool IsIconOverride { get; set; }
    partial void OnIsIconOverrideChanged(bool value)
    {
        var uri = TmpIcon?.ToString();
        IconOverride = value ? _defaultIconOverride ??= new IconOverride { Uri = uri is null ? null : new Uri(uri) } : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial IconOverride? IconOverride { get; set; }

    #endregion Icon Override

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool PersistSettings { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnPersistSettingsChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    #region ProxyEnabled
    [JsonIgnore]
    [ObservableProperty]
    public partial bool ProxyEnabled { get; set; }
    partial void OnProxyEnabledChanged(bool value)
    {
        Proxy = value && (!string.IsNullOrEmpty(ProxyUri) || !string.IsNullOrEmpty(ProxyUserName) || !string.IsNullOrEmpty(ProxyPassword)) ? _defaultProxy ??= new ProxyEnabled() : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? ProxyUri { get; set; }
    partial void OnProxyUriChanged(string? value)
    {
        Proxy      = !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(ProxyUserName) || !string.IsNullOrEmpty(ProxyPassword) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.Uri = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? ProxyUserName { get; set; }
    partial void OnProxyUserNameChanged(string? value)
    {
        Proxy           = !string.IsNullOrEmpty(ProxyUri) || !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(ProxyPassword) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.UserName = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? ProxyPassword { get; set; }
    partial void OnProxyPasswordChanged(string? value)
    {
        Proxy           = !string.IsNullOrEmpty(ProxyUri) || !string.IsNullOrEmpty(ProxyUserName) || !string.IsNullOrEmpty(value) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.Password = !string.IsNullOrEmpty(value) ? value : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial ProxyEnabled? Proxy { get; set; }
    #endregion ProxyEnabled

    /// <summary>
    ///     AutoUpdater.NET will report errors if this is true.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool ReportErrors { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnReportErrorsChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    /// <summary>
    ///     Set TopMostDisabled to true for all updater dialogs.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool TopMostDisabled { get; set; } = true;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTopMostDisabledChanged(bool value) => UpdateValidation?.Invoke(!Equals());

    #region Use ZipFile
    [JsonIgnore]
    [ObservableProperty]
    public partial bool UseZipFile { get; set; }
    partial void OnUseZipFileChanged(bool value)
    {
        ZipFile = !value ? null : new ZipFile(); // _clearAppDirectory || (_executablePathOverride && !string.IsNullOrEmpty(_executablePath)) || (_zipExtractionPathOverride && !string.IsNullOrEmpty(_installationPath)) ? _defaultZipFile ??= new ZipFile() : null;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     Set this to true if you want to clear application directory before extracting update.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial bool ClearAppDirectory { get; set; }
    partial void OnClearAppDirectoryChanged(bool value)
    {
        ZipFile                    = ClearAppDirectory || (ExecutablePathOverride && !string.IsNullOrEmpty(ExecutablePath)) || (ZipExtractionPathOverride && !string.IsNullOrEmpty(InstallationPath)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ClearAppDirectory = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial bool ExecutablePathOverride { get; set; }
    partial void OnExecutablePathOverrideChanged(bool value)
    {
        ZipFile                               = ClearAppDirectory || (ExecutablePathOverride && !string.IsNullOrEmpty(ExecutablePath)) || (ZipExtractionPathOverride && !string.IsNullOrEmpty(InstallationPath)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ExecutablePathOverride       = value && !string.IsNullOrEmpty(ExecutablePath) ? new FilePath() : null;
        ZipFile?.ExecutablePathOverride?.Path = ExecutablePath;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     If you are using a zip file as an update file, then you can set this value to a new executable path relative to the
    ///     installation directory.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial string? ExecutablePath { get; set; }
    partial void OnExecutablePathChanged(string? value)
    {
        ZipFile                               = ClearAppDirectory || (ExecutablePathOverride && !string.IsNullOrEmpty(value)) || (ZipExtractionPathOverride && !string.IsNullOrEmpty(InstallationPath)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ExecutablePathOverride       = !string.IsNullOrEmpty(value) ? new FilePath() : null;
        ZipFile?.ExecutablePathOverride?.Path = value;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial bool ZipExtractionPathOverride { get; set; }
    partial void OnZipExtractionPathOverrideChanged(bool value)
    {
        ZipFile                                  = ClearAppDirectory || (ExecutablePathOverride && !string.IsNullOrEmpty(ExecutablePath)) || (ZipExtractionPathOverride && !string.IsNullOrEmpty(InstallationPath)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ZipExtractionPathOverride       = value && !string.IsNullOrEmpty(InstallationPath) ? new FilePath() : null;
        ZipFile?.ZipExtractionPathOverride?.Path = InstallationPath;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     If you are using a zip file as an update file then you can set this value to path where your app is installed. This
    ///     is only necessary when your installation directory differs from your executable path.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial string? InstallationPath { get; set; }
    partial void OnInstallationPathChanged(string? value)
    {
        ZipFile                                  = ClearAppDirectory || (ExecutablePathOverride && !string.IsNullOrEmpty(ExecutablePath)) || (ZipExtractionPathOverride && !string.IsNullOrEmpty(value)) ? _defaultZipFile ??= new ZipFile() : null;
        ZipFile?.ZipExtractionPathOverride       = !string.IsNullOrEmpty(value) ? new FilePath() : null;
        ZipFile?.ZipExtractionPathOverride?.Path = value;

        UpdateValidation?.Invoke(!Equals());
    }

    #endregion Use ZipFile

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial ZipFile? ZipFile { get; set; }

    #region TimerEnabled

    [JsonIgnore]
    [ObservableProperty]
    public partial bool TimerEnabled { get; set; }
    partial void OnTimerEnabledChanged(bool value)
    {
        Timer = value ? _defaultTimer ??= new TimerEnabled() : null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial ushort TimerInterval { get; set; } = 1;
    partial void OnTimerIntervalChanged(ushort value)
    {
        Timer?.Interval = value;

        if (_defaultTimer == Timer)
            _defaultTimer = null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial RemindLaterFormat TimerDurationTimeSpan { get; set; } = RemindLaterFormat.Seconds;
    partial void OnTimerDurationTimeSpanChanged(RemindLaterFormat value)
    {
        Timer?.TimeSpan = value;

        if (_defaultTimer == Timer)
            _defaultTimer = null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial TimerEnabled? Timer { get; set; }
    #endregion TimerEnabled

    #region UserSelectRemindLater

    [JsonIgnore]
    [ObservableProperty]
    public partial bool UserSelectRemindLater { get; set; }
    partial void OnUserSelectRemindLaterChanged(bool value)
    {
        RemmindLaterTimer = value ? _defaultRemindLaterTimer ??= new TimerEnabled() : null;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     Remind Later interval after user should be reminded of update.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial ushort RemindLaterAt { get; set; } = 1;
    partial void OnRemindLaterAtChanged(ushort value)
    {
        RemmindLaterTimer?.Interval = value;

        if (_defaultRemindLaterTimer == Timer)
            _defaultRemindLaterTimer = null;

        UpdateValidation?.Invoke(!Equals());
    }

    /// <summary>
    ///     Set if RemindLaterAt interval should be in Minutes, Hours or Days.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial RemindLaterFormat RemindLaterTimeSpan { get; set; } = RemindLaterFormat.Minutes;
    partial void OnRemindLaterTimeSpanChanged(RemindLaterFormat value)
    {
        RemmindLaterTimer?.TimeSpan = value;

        if (_defaultRemindLaterTimer == Timer)
            _defaultRemindLaterTimer = null;

        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UserSelectRemmindLater")]
    [ObservableProperty]
    public partial TimerEnabled? RemmindLaterTimer { get; set; }

    #endregion UserSelectRemindLater

    #region Version

    /// <summary>
    ///     You can set this field to your current version if you don't want to determine the version from the assembly.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial InstalledVersion? InstalledVersion { get; set; }

    [JsonIgnore]
    [ObservableProperty]
    public partial bool InstalledVersionOverride { get; set; }
    partial void OnInstalledVersionOverrideChanged(bool value)
    {
        InstalledVersion = value ? new InstalledVersion { Version = new Version(MajorVersion, MinorVersion, BuildVersion, RevisionVersion) } : null;
        UpdateValidation?.Invoke(!Equals());
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial ushort MajorVersion { get; set; } = (ushort)typeof(ViewModelMainConfig).Assembly.Version()!.Major;
    partial void OnMajorVersionChanged(ushort value)
    {
        var tmpVer = new Version(value, MinorVersion, BuildVersion, RevisionVersion);
        SetInstalledVersion(tmpVer);
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial ushort MinorVersion { get; set; } = (ushort)typeof(ViewModelMainConfig).Assembly.Version()!.Minor;
    partial void OnMinorVersionChanged(ushort value)
    {
        var tmpVer = new Version(MajorVersion, value, BuildVersion, RevisionVersion);
        SetInstalledVersion(tmpVer);
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial ushort BuildVersion { get; set; } = (ushort)typeof(ViewModelMainConfig).Assembly.Version()!.Build;
    partial void OnBuildVersionChanged(ushort value)
    {
        var tmpVer = new Version(MajorVersion, MinorVersion, value, RevisionVersion);
        SetInstalledVersion(tmpVer);
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial ushort RevisionVersion { get; set; } = (ushort)typeof(ViewModelMainConfig).Assembly.Version()!.Revision;
    partial void OnRevisionVersionChanged(ushort value)
    {
        var tmpVer = new Version(MajorVersion, MinorVersion, BuildVersion, value);
        SetInstalledVersion(tmpVer);
    }

    #endregion Version

    [JsonIgnore]
    [ObservableProperty]
    public partial BitmapImage? TmpIcon { get; set; } = null;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTmpIconChanged(BitmapImage? value) => UpdateValidation?.Invoke(!Equals());

    [ObservableProperty]
    [JsonIgnore]
    public partial ObservableCollection<TreeViewItem> TimerNodeList { get; set; } = [];

    [ObservableProperty]
    [JsonIgnore]
    public partial ObservableCollection<TreeViewItem> UpdateCompleteNodeList { get; set; } = [];

    [ObservableProperty]
    [JsonIgnore]
    public partial ObservableCollection<TreeViewItem> CheckForUpdatesNodeList { get; set; } = [];

    [ObservableProperty]
    [JsonIgnore]
    public partial ObservableCollection<TreeViewItem> ParseUpdateInfoNodeList { get; set; } = [];

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
    public ViewModelMainConfig(IServiceProvider serviceProvider)
    {
        TmpIcon              = Application.Current!.Resources["Project"] as BitmapImage;
        _defaultIconOverride = new IconOverride
        {
            Uri = string.IsNullOrEmpty(TmpIcon?.ToString()) ? null : new Uri(TmpIcon.ToString()!)
        };
        _defaultInstalledVersion = Assembly.GetExecutingAssembly().Version()!;
        _serviceProvider         = serviceProvider;
    }


    /// <summary>
    ///     Copy Constructor
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="serviceProvider"></param>
    public ViewModelMainConfig(ViewModelConfig parent, IServiceProvider serviceProvider) : this(serviceProvider)
    {
        Copy(parent);
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
}