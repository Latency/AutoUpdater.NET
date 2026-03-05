// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Config.cs
// Author:   Latency McLaughlin
// Date:     06/18/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming
#pragma warning disable CS0657 // Not a valid attribute location for this declaration

using AssemblyLoader;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AutoUpdaterDotNET.Models;

internal sealed partial class Config : ObservableObject, IConfig, ICloneable
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private Version2 _defaultInstalledVersion;

    private string? _defaultAppTitle;

    private readonly bool[] _defaultMandatory = new bool[2];

    private ProxyEnabled? _defaultProxy;

    private TimerEnabled? _defaultTimer, _defaultRemindLaterTimer;

    private IsMandatory? _defaultIsMandatory;

    private WindowSize? _defaultWindowSize;

    private IconOverride? _defaultIconOverride;

    private BasicAuth? _defaultBasicAuth;

    internal Func<bool>? EqualsPredicate;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #region FTP

    [JsonIgnore]
    public FtpProfile2? FtpProfile { get; set; } = new();

    #endregion FTP

    #region WindowSize

    [JsonIgnore]
    [ObservableProperty]
    public partial bool WindowSizeOverride { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnWindowSizeOverrideChanged(bool value)
    {
        if (value)
        {
            WindowSize                 ??= _defaultWindowSize;
            WindowSize?.PropertyChanged +=  OnUpdateValidation;
        }
        else
        {
            if (WindowSize is not null)
                _defaultWindowSize = WindowSize;

            WindowSize?.PropertyChanged -= OnUpdateValidation;
            WindowSize                  =  null;
        }

        return;

        void OnUpdateValidation(object? sender, PropertyChangedEventArgs e) => _UpdateValidation();
    }

    /// <summary>
    ///     Resizes the update window.
    /// </summary>
    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public partial WindowSize? WindowSize { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnWindowSizeChanged(WindowSize? value) => _UpdateValidation();

    #endregion WindowSize

    #region Version

    [JsonIgnore]
    [ObservableProperty]
    public partial bool InstalledVersionOverride { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnInstalledVersionOverrideChanged(bool value)
    {
        if (value)
        {
            InstalledVersion                 ??= _defaultInstalledVersion;
            InstalledVersion.PropertyChanged +=  OnUpdateValidation;
        }
        else
        {
            if (InstalledVersion is not null)
                _defaultInstalledVersion = new(InstalledVersion);

            InstalledVersion?.PropertyChanged -= OnUpdateValidation;
            InstalledVersion                  =  null;
        }

        return;

        void OnUpdateValidation(object? sender, PropertyChangedEventArgs e) => _UpdateValidation();
    }

    /// <summary>
    ///     You can set this field to your current version if you don't want to determine the version from the assembly.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial Version2? InstalledVersion { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnInstalledVersionChanged(Version2? value) => _UpdateValidation();

    #endregion Version

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
        if (value && AppTitle == _defaultAppTitle)
            return;
        AppTitle = value ? _defaultAppTitle : null;

        _UpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial string? AppTitle { get; set; }
    partial void OnAppTitleChanged(string? value)
    {
        if (value is not null)
        {
            _defaultAppTitle = value;
            if (!IsAppTitle)
                IsAppTitle = true;
        }

        _UpdateValidation();
    }
    #endregion AppTitle

    #region IsMandatory
    [JsonIgnore]
    [ObservableProperty]
    public partial bool IsMandatory { get; set; }
    partial void OnIsMandatoryChanged(bool value)
    {
        Mandatory = value ? _defaultIsMandatory ??= new IsMandatory() : null;

        if (Mandatory is null)
        {
            ShowSkipButton = _defaultMandatory[0];
            ShowRemindLaterButton = _defaultMandatory[1];

            if (!(ShowSkipButton | ShowRemindLaterButton))
                ShowSkipButton = true;
        }
        else
        {
            ShowSkipButton = false;
            ShowRemindLaterButton = false;
        }

        _UpdateValidation();
    }

    /// <summary>
    ///     If this is true users can see the skip button.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool ShowSkipButton { get; set; } = true;
    partial void OnShowSkipButtonChanged(bool value)
    {
        _defaultMandatory[0] = value;

        if (!(value | ShowRemindLaterButton))
            IsMandatory = true;

        _UpdateValidation();
    }

    /// <summary>
    ///     If this is true users can see the Remind Later button.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool ShowRemindLaterButton { get; set; } = true;
    partial void OnShowRemindLaterButtonChanged(bool value)
    {
        _defaultMandatory[1] = value;

        if (!(value | ShowSkipButton))
            IsMandatory = true;

        _UpdateValidation();
    }

    /// <summary>
    ///     Set this to any of the available modes to change behaviour of the Mandatory flag.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial Mode UpdateMode { get; set; } = Mode.Normal;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnUpdateModeChanged(Mode value)
    {
        Mandatory?.UpdateMode = value;

        if (_defaultIsMandatory == Mandatory)
            _defaultIsMandatory = null;

        _UpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial IsMandatory? Mandatory { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    //partial void OnMandatoryChanged(IsMandatory value) => _UpdateValidation();
    #endregion IsMandatory

    /// <summary>
    ///     Set this to false if your application doesn't need administrator privileges to replace the old version.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool RunUpdateAsAdmin { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnRunUpdateAsAdminChanged(bool value) => _UpdateValidation();

    /// <summary>
    ///     Opens the download URL in default browser if true. Very useful if you have portable application.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool OpenDownloadPage { get; set; } = true;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnOpenDownloadPageChanged(bool value) => _UpdateValidation();

    #region Basic Auth

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BasicAuthentication")]
    [ObservableProperty]
    public partial BasicAuth? BasicAuth { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    //partial void OnBasicAuthChanged(BasicAuth value) => _UpdateValidation();

    [JsonIgnore]
    [ObservableProperty]
    public partial bool IsBasicAuth { get; set; }
    partial void OnIsBasicAuthChanged(bool value)
    {
        BasicAuth = value && (BasicAuthChangeLog || BasicAuthDownload || !string.IsNullOrEmpty(BasicAuthUserName) || !string.IsNullOrEmpty(BasicAuthPassword)) ? _defaultBasicAuth ??= new BasicAuth() : null;

        _UpdateValidation();
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

        _UpdateValidation();
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

        _UpdateValidation();
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? BasicAuthUserName { get; set; }
    partial void OnBasicAuthUserNameChanged(string? value)
    {
        BasicAuth           = BasicAuthChangeLog || BasicAuthDownload || !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(BasicAuthPassword) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.UserName = !string.IsNullOrEmpty(value) ? value : null;

        _UpdateValidation();
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? BasicAuthPassword { get; set; }
    partial void OnBasicAuthPasswordChanged(string? value)
    {
        BasicAuth           = BasicAuthChangeLog || BasicAuthDownload || !string.IsNullOrEmpty(BasicAuthUserName) || !string.IsNullOrEmpty(value) ? _defaultBasicAuth ??= new BasicAuth() : null;
        BasicAuth?.Password = !string.IsNullOrEmpty(value) ? value : null;

        _UpdateValidation();
    }
    #endregion Basic Auth

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool DoNotBindOwnerWindow { get; set; } = true;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnDoNotBindOwnerWindowChanged(bool value) => _UpdateValidation();

    /// <summary>
    ///     Set this to true if you want to run update check synchronously.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool CheckSynchronously { get; set; } = true;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnCheckSynchronouslyChanged(bool value) => _UpdateValidation();

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool FtpProtocol { get; set; } = true;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnFtpProtocolChanged(bool value) => _UpdateValidation();

    #region Icon Override
    [JsonIgnore]
    [ObservableProperty]
    public partial bool IsIconOverride { get; set; }
    partial void OnIsIconOverrideChanged(bool value)
    {
        var uri = TmpIcon?.ToString();
        IconOverride = value ? _defaultIconOverride ??= new IconOverride { Uri = uri is null ? null : new Uri(uri) } : null;

        _UpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial IconOverride? IconOverride { get; set; }

    #endregion Icon Override

    #region ProxyEnabled
    [JsonIgnore]
    [ObservableProperty]
    public partial bool ProxyEnabled { get; set; }
    partial void OnProxyEnabledChanged(bool value)
    {
        Proxy = value && (!string.IsNullOrEmpty(ProxyUri) || !string.IsNullOrEmpty(ProxyUserName) || !string.IsNullOrEmpty(ProxyPassword)) ? _defaultProxy ??= new ProxyEnabled() : null;

        _UpdateValidation();
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? ProxyUri { get; set; }
    partial void OnProxyUriChanged(string? value)
    {
        Proxy      = !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(ProxyUserName) || !string.IsNullOrEmpty(ProxyPassword) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.Uri = !string.IsNullOrEmpty(value) ? value : null;

        _UpdateValidation();
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? ProxyUserName { get; set; }
    partial void OnProxyUserNameChanged(string? value)
    {
        Proxy           = !string.IsNullOrEmpty(ProxyUri) || !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(ProxyPassword) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.UserName = !string.IsNullOrEmpty(value) ? value : null;

        _UpdateValidation();
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial string? ProxyPassword { get; set; }
    partial void OnProxyPasswordChanged(string? value)
    {
        Proxy           = !string.IsNullOrEmpty(ProxyUri) || !string.IsNullOrEmpty(ProxyUserName) || !string.IsNullOrEmpty(value) ? _defaultProxy ??= new ProxyEnabled() : null;
        Proxy?.Password = !string.IsNullOrEmpty(value) ? value : null;

        _UpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial ProxyEnabled? Proxy { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    //partial void OnProxyChanged(bool value) => _UpdateValidation();
    #endregion ProxyEnabled

    /// <summary>
    ///     AutoUpdater.NET will report errors if this is true.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool ReportErrors { get; set; } = true;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnReportErrorsChanged(bool value) => _UpdateValidation();

    /// <summary>
    ///     Set TopMostDisabled to true for all updater dialogs.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [ObservableProperty]
    public partial bool TopMostDisabled { get; set; } = true;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTopMostDisabledChanged(bool value) => _UpdateValidation();

    /// <summary>
    ///     Checksum of the update file.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial CheckSum? CheckSum { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnCheckSumChanged(CheckSum? value) => _UpdateValidation();

    /// <summary>
    ///     Set this to true if you want to clear application directory before extracting the update.
    /// </summary>
    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public partial bool ClearAppDirectory { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnClearAppDirectoryChanged(bool value) => _UpdateValidation();

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial FilePath? ExecutablePathOverride { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnExecutablePathOverrideChanged(FilePath? value) => _UpdateValidation();

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial FilePath? InstallationPathOverride { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnInstallationPathOverrideChanged(FilePath? value) => _UpdateValidation();

    #region TimerEnabled

    [JsonIgnore]
    [ObservableProperty]
    public partial bool TimerEnabled { get; set; }
    partial void OnTimerEnabledChanged(bool value)
    {
        Timer = value ? _defaultTimer ??= new TimerEnabled() : null;

        _UpdateValidation();
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial ushort TimerInterval { get; set; } = 1;
    partial void OnTimerIntervalChanged(ushort value)
    {
        Timer?.Interval = value;

        if (_defaultTimer == Timer)
            _defaultTimer = null;

        _UpdateValidation();
    }

    [JsonIgnore]
    [ObservableProperty]
    public partial RemindLaterFormat TimerDurationTimeSpan { get; set; } = RemindLaterFormat.Seconds;
    partial void OnTimerDurationTimeSpanChanged(RemindLaterFormat value)
    {
        Timer?.TimeSpan = value;

        if (_defaultTimer == Timer)
            _defaultTimer = null;

        _UpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ObservableProperty]
    public partial TimerEnabled? Timer { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    //partial void OnTimerChanged(TimerEnabled value) => _UpdateValidation();
    #endregion TimerEnabled

    #region UserSelectRemindLater

    /// <summary>
    ///     If this is true users see dialog where they can set remind later interval otherwise it will take the interval from
    ///     RemindLaterAt and RemindLaterTimeSpan fields.
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial bool UserSelectRemindLater { get; set; } = true;
    partial void OnUserSelectRemindLaterChanged(bool value)
    {
        RemmindLaterTimer = value ? _defaultRemindLaterTimer ??= new TimerEnabled() : null;

        _UpdateValidation();
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

        _UpdateValidation();
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

        _UpdateValidation();
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UserSelectRemmindLater")]
    [ObservableProperty]
    public partial TimerEnabled? RemmindLaterTimer { get; set; }
    // ReSharper disable once UnusedParameterInPartialMethod
    //partial void OnRemmindLaterTimerChanged(TimerEnabled value) => _UpdateValidation();

    #endregion UserSelectRemindLater

    [JsonIgnore]
    [ObservableProperty]
    public partial BitmapImage? TmpIcon { get; set; } = null;
    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnTmpIconChanged(BitmapImage? value) => _UpdateValidation();

    [ObservableProperty]
    [JsonIgnore]
    public partial ObservableCollection<TreeViewItem>? TimerNodeList { get; set; }

    [ObservableProperty]
    [JsonIgnore]
    public partial ObservableCollection<TreeViewItem>? UpdateCompleteNodeList { get; set; }

    [ObservableProperty]
    [JsonIgnore]
    public partial ObservableCollection<TreeViewItem>? BeforeCheckForUpdatesNodeList { get; set; }

    [ObservableProperty]
    [JsonIgnore]
    public partial ObservableCollection<TreeViewItem>? AfterCheckForUpdatesNodeList { get; set; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    internal event Action<bool?>?        UpdateValidation;
    internal event Action<string?>?      UpdateVersion;
    internal event Action<BitmapImage?>? UpdateIcon;
    internal event Action<string?>?      UpdateTitle;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    #region Event Invocators
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private  void _UpdateValidation() => _UpdateValidation(!EqualsPredicate?.Invoke());

    internal void _UpdateValidation(bool? value) => UpdateValidation?.Invoke(value);

    internal void _UpdateTitle() => UpdateTitle?.Invoke(AppTitle);

    internal void _UpdateVersion() => UpdateVersion?.Invoke($"Loader Version: {InstalledVersion ?? _defaultInstalledVersion}");

    internal void _UpdateIcon(BitmapImage? value)
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
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public Config()
    {
        TmpIcon = Application.Current!.Resources["Project"] as BitmapImage;
        _defaultIconOverride = new IconOverride
        {
            Uri = string.IsNullOrEmpty(TmpIcon?.ToString()) ? null : new Uri(TmpIcon.ToString()!)
        };
        _defaultInstalledVersion = (Version2) Assembly.GetExecutingAssembly().Version()!;
    }


    /// <summary>
    ///     Copy Constructor (Overload +2)
    /// </summary>
    /// <param name="other"></param>
    internal Config(IConfig other) : this() => Clone(other);

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Constructors


    #region Methods
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public object Clone() => new Config(this);

    internal void Clone(IConfig other)
    {
        foreach (var prop in other.GetType().GetProperties())
        {
            if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(BitmapImage))
                GetType().GetProperty(prop.Name)?.SetValue(this, prop.GetValue(other));
            else
            {
                var b = prop.GetValue(other);
                if (b is not null)
                    GetType().GetProperty(prop.Name)!.SetValue(this, Activator.CreateInstance(prop.PropertyType, b));
            }
        }

        EqualsPredicate               = ((Config)other).EqualsPredicate;
        BeforeCheckForUpdatesNodeList = other.BeforeCheckForUpdatesNodeList;
        AfterCheckForUpdatesNodeList  = other.AfterCheckForUpdatesNodeList;
        TimerNodeList                 = other.TimerNodeList;
        UpdateCompleteNodeList        = other.UpdateCompleteNodeList;
        InstalledVersionOverride      = other.InstalledVersion != null;
        WindowSizeOverride            = other.WindowSize       != null;
    }


    public bool Equals(IConfig? other)
    {
        if (other == null)
            return false;

        return IsVersionOverride()    &&
               IsMandatory()          &&
               IsIconOverride()       &&
               IsWindowSizeOverride() &&
               AppTitle                  == other.AppTitle &&
               IsAppTitle                == other.IsAppTitle &&
               BasicAuthPassword         == other.BasicAuthPassword &&
               BasicAuthUserName         == other.BasicAuthUserName &&
               CheckSum                  == other.CheckSum &&
               ExecutablePathOverride    == other.ExecutablePathOverride &&
               InstallationPathOverride  == other.InstallationPathOverride &&
               Proxy?.Uri                == other.Proxy?.Uri &&
               Proxy?.Password           == other.Proxy?.Password &&
               Proxy?.UserName           == other.Proxy?.UserName &&
               BasicAuth                 == other.BasicAuth &&
               BasicAuthChangeLog        == other.BasicAuthChangeLog &&
               BasicAuthDownload         == other.BasicAuthDownload &&
               CheckSynchronously        == other.CheckSynchronously &&
               ClearAppDirectory         == other.ClearAppDirectory &&
               DoNotBindOwnerWindow      == other.DoNotBindOwnerWindow &&
               FtpProtocol               == other.FtpProtocol &&
               OpenDownloadPage          == other.OpenDownloadPage &&
               ProxyEnabled              == other.ProxyEnabled &&
               RemindLaterAt             == other.RemindLaterAt &&
               RemindLaterTimeSpan       == other.RemindLaterTimeSpan &&
               ReportErrors              == other.ReportErrors &&
               RunUpdateAsAdmin          == other.RunUpdateAsAdmin &&
               TimerDurationTimeSpan     == other.TimerDurationTimeSpan &&
               TimerInterval             == other.TimerInterval &&
               TopMostDisabled           == other.TopMostDisabled &&
               UserSelectRemindLater     == other.UserSelectRemindLater;


        bool IsIconOverride() => TmpIcon is not null && other.TmpIcon is not null ? TmpIcon.IsEqual(other.TmpIcon) : this.IsIconOverride == other.IsIconOverride;

        bool IsWindowSizeOverride()
        {
            if (WindowSize is not null && other.WindowSize is not null)
            {
                return
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    WindowSize.Width  == other.WindowSize.Width &&
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    WindowSize.Height == other.WindowSize.Height;
            }

            return WindowSizeOverride == other.WindowSizeOverride;
        }

        bool IsVersionOverride()
        {
            if (InstalledVersion is not null && other.InstalledVersion is not null)
            {
                return
                    InstalledVersion.Major    == other.InstalledVersion.Major &&
                    InstalledVersion.Minor    == other.InstalledVersion.Minor &&
                    InstalledVersion.Build    == other.InstalledVersion.Build &&
                    InstalledVersion.Revision == other.InstalledVersion.Revision;
            }

            return InstalledVersionOverride == other.InstalledVersionOverride;
        }

        bool IsMandatory()
        {
            if (!other.IsMandatory)
                return ShowRemindLaterButton == other.ShowRemindLaterButton &&
                       ShowSkipButton        == other.ShowSkipButton        &&
                       this.IsMandatory      == other.IsMandatory;

            return UpdateMode       == other.UpdateMode &&
                   this.IsMandatory == other.IsMandatory;
        }
    }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}