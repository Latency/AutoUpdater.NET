using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Persistance_Providers;
using Timer = System.Timers.Timer;

namespace AutoUpdaterDotNET;

/// <summary>
///     Main class that lets you auto update applications by setting some fields and executing its Start method.
/// </summary>
public sealed class AutoUpdater : Window
{
    private static readonly Lazy<AutoUpdater> SingletonAutoUpdater = new(() => new());
    public static           AutoUpdater       Instance => SingletonAutoUpdater.Value;


    private static readonly Lazy<HttpClientHandler> SingletonHttpClientHandler = new(() => new()
    {
        Credentials             = CredentialCache.DefaultCredentials,
        PreAuthenticate         = true,
        AllowAutoRedirect       = true,
        MaxConnectionsPerServer = 1,
        UseCookies              = false,
        AutomaticDecompression  = DecompressionMethods.GZip,
        UseDefaultCredentials   = true,
        UseProxy                = false,
        DefaultProxyCredentials = new CredentialCache()
    });
    private static HttpClientHandler HttpClientHandlerInstance => SingletonHttpClientHandler.Value;

    private HttpClient HttpWebClient { get; }


    /// <summary>
    ///     Singleton Default Constructor
    /// </summary>
    private AutoUpdater()
    {
        HttpWebClient = new HttpClient(HttpClientHandlerInstance);
    }


    /// <summary>
    ///     A delegate type to handle how to exit the application after update is downloaded.
    /// </summary>
    public delegate void ApplicationExitEventHandler();

    /// <summary>
    ///     A delegate type for hooking up update notifications.
    /// </summary>
    /// <param name="args">
    ///     An object containing all the parameters received from AppCast XML file. If there will be an error
    ///     while looking for the XML file then this object will be null.
    /// </param>
    public delegate void CheckForUpdateEventHandler(UpdateInfoEventArgs args);

    /// <summary>
    ///     A delegate type for hooking up parsing logic.
    /// </summary>
    /// <param name="args">An object containing the AppCast file received from server.</param>
    public delegate void ParseUpdateInfoHandler(ParseUpdateInfoEventArgs args);


    private Timer? _remindLaterTimer;

    internal Uri BaseUri
    {
        get;
        set
        {
            field                                 = value;
            HttpClientHandlerInstance.Credentials = field.Scheme.Equals(Uri.UriSchemeFtp) ? FtpCredentials : CredentialCache.DefaultCredentials;
        }
    }

    internal bool Running;

    /// <summary>
    ///     URL of the xml file that contains information about latest version of the application.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public string AppCastURL { get; set; }

    /// <summary>
    ///     Set the Application Title shown in Update dialog. Although AutoUpdater.NET will get it automatically, you can set
    ///     this property if you like to give custom Title.
    /// </summary>
    public string? AppTitle;

    /// <summary>
    ///     Set Basic Authentication credentials to navigate to the change log URL.
    /// </summary>
    public ICredentials BasicAuthChangeLog;

    /// <summary>
    ///     Set Basic Authentication credentials required to download the file.
    /// </summary>
    public ICredentials BasicAuthDownload;

    /// <summary>
    ///     Set Basic Authentication credentials required to download the XML file.
    /// </summary>
    public AuthenticationHeaderValue BasicAuthHeaderValue;

    /// <summary>
    ///     Set this to true if you want to clear application directory before extracting update.
    /// </summary>
    public bool ClearAppDirectory = false;

    /// <summary>
    ///     Set it to folder path where you want to download the update file. If not provided then it defaults to Temp folder.
    /// </summary>
    public string DownloadPath;

    /// <summary>
    ///     If you are using a zip file as an update file, then you can set this value to a new executable path relative to the
    ///     installation directory.
    /// </summary>
    public string ExecutablePath;

    /// <summary>
    ///     Login/password/domain for FTP-request
    /// </summary>
    public NetworkCredential? FtpCredentials { get; set; }

    /// <summary>
    ///     Set the User-Agent string to be used for HTTP web requests.
    /// </summary>
    public string HttpUserAgent;

    /// <summary>
    ///     If you are using a zip file as an update file then you can set this value to path where your app is installed. This
    ///     is only necessary when your installation directory differs from your executable path.
    /// </summary>
    public string? InstallationPath;

    /// <summary>
    ///     You can set this field to your current version if you don't want to determine the version from the assembly.
    /// </summary>
    public Version? InstalledVersion;

    /// <summary>
    ///     If this is true users see dialog where they can set remind later interval otherwise it will take the interval from
    ///     RemindLaterAt and RemindLaterTimeSpan fields.
    /// </summary>
    public bool LetUserSelectRemindLater = true;

    /// <summary>
    ///     Set this to true if you want to ignore previously assigned Remind Later and Skip settings. It will also hide Remind
    ///     Later and Skip buttons.
    /// </summary>
    public bool Mandatory;

    /// <summary>
    ///     Opens the download URL in default browser if true. Very useful if you have portable application.
    /// </summary>
    public bool OpenDownloadPage;

    /// <summary>
    ///     Set this to an instance implementing the IPersistenceProvider interface for using a data storage method different
    ///     from the default Windows Registry based one.
    /// </summary>
    public IPersistenceProvider PersistenceProvider;

    /// <summary>
    ///     Remind Later interval after user should be reminded of update.
    /// </summary>
    public int RemindLaterAt { get; init; } = 2;

    /// <summary>
    ///     Set if RemindLaterAt interval should be in Minutes, Hours or Days.
    /// </summary>
    public RemindLaterFormat RemindLaterTimeSpan { get; init; } = RemindLaterFormat.Days;

    /// <summary>
    ///     AutoUpdater.NET will report errors if this is true.
    /// </summary>
    public bool ReportErrors { get; init; } = false;

    /// <summary>
    ///     Set this to false if your application doesn't need administrator privileges to replace the old version.
    /// </summary>
    public bool RunUpdateAsAdmin { get; init; } = true;

    /// <summary>
    ///     If this is true users can see the Remind Later button.
    /// </summary>
    public bool ShowRemindLaterButton { get; private set; } = true;

    /// <summary>
    ///     If this is true users can see the skip button.
    /// </summary>
    public bool ShowSkipButton { get; private set; } = true;

    /// <summary>
    ///     Set this to true if you want to run update check synchronously.
    /// </summary>
    public bool Synchronous { get; init; } = false;

    /// <summary>
    ///     Set this to any of the available modes to change behaviour of the Mandatory flag.
    /// </summary>
    public Mode UpdateMode;

    /// <summary>
    ///     An event that developers can use to exit the application gracefully.
    /// </summary>
    public event ApplicationExitEventHandler? ApplicationExitEvent;

    /// <summary>
    ///     An event that clients can use to be notified whenever the update is checked.
    /// </summary>
    public event CheckForUpdateEventHandler? CheckForUpdateEvent;

    /// <summary>
    ///     An event that clients can use to be notified whenever the AppCast file needs parsing.
    /// </summary>
    public event ParseUpdateInfoHandler? ParseUpdateInfoEvent;

    /// <summary>
    ///     Start checking for new version of application and display a dialog to the user if update is available.
    /// </summary>
    /// <param name="myAssembly">Assembly to use for version checking.</param>
    public void Start(Assembly? myAssembly = null)
    {
        _ = Start(AppCastURL, myAssembly);
    }

    /// <summary>
    ///     Start checking for new version of application via FTP and display a dialog to the user if update is available.
    /// </summary>
    /// <param name="appCast">FTP URL of the xml file that contains information about latest version of the application.</param>
    /// <param name="ftpCredentials">Credentials required to connect to FTP server.</param>
    /// <param name="myAssembly">Assembly to use for version checking.</param>
    public void Start(string appCast, NetworkCredential ftpCredentials, Assembly? myAssembly = null)
    {
        FtpCredentials = ftpCredentials;
        _ = Start(appCast, myAssembly);
    }

    /// <summary>
    ///     Start checking for new version of application and display a dialog to the user if update is available.
    /// </summary>
    /// <param name="appCast">URL of the xml file that contains information about latest version of the application.</param>
    /// <param name="myAssembly">Assembly to use for version checking.</param>
    public async Task Start(string appCast, Assembly? myAssembly = null)
    {
        if (Mandatory && _remindLaterTimer != null)
        {
            _remindLaterTimer.Stop();
            _remindLaterTimer.Close();
            _remindLaterTimer = null;
        }

        if (Running || _remindLaterTimer != null)
            return;

        Running = true;

        AppCastURL = appCast;

        var assembly = myAssembly ?? Assembly.GetEntryAssembly()!;

        if (Synchronous)
        {
            try
            {
                var args = CheckUpdate(assembly).Result;

                // Change the window size if overriden.
                if (args?.UpdateFormSize != null)
                {
                    Height = args.UpdateFormSize.Value.Height;
                    Width  = args.UpdateFormSize.Value.Width;
                }

                if (StartUpdate(args))
                    return;

                Running = false;
            }
            catch (Exception exception)
            {
                ShowError(exception);
            }
        }
        else
        {
            try {
                await Task.Run(() => CheckUpdate(assembly))
                      .ContinueWith(t =>
                      {
                          var args = t.Result;

                          // Change the window size if overriden.
                          if (args?.UpdateFormSize != null)
                          {
                              Height = args.UpdateFormSize.Value.Height;
                              Width  = args.UpdateFormSize.Value.Width;
                          }

                          if (args?.Error != null)
                              ShowError(args.Error);
                          else
                          {
                              if (!t.IsCanceled && StartUpdate(args))
                                  return;

                              Running = false;
                          }
                      }).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                // Handled
            }
            catch (Exception exception)
            {
                ShowError(exception);
            }
        }
    }


    /// <summary>
    ///     Set Proxy server to use for all the web requests in AutoUpdater.NET.
    /// </summary>
    private void Proxy(string username, string password, string address)
    {
        HttpClientHandlerInstance.Proxy = new WebProxy
        {
            Address = new Uri(address)
        };
        HttpClientHandlerInstance.DefaultProxyCredentials = new NetworkCredential(username, password);
        HttpClientHandlerInstance.UseProxy                = true;
    }


    private async Task<UpdateInfoEventArgs?> CheckUpdate(Assembly mainAssembly)
    {
        var appCompany = AssemblyLoader.AssemblyInfo.Company(mainAssembly);

        if (string.IsNullOrEmpty(AppTitle))
            AppTitle = AssemblyLoader.AssemblyInfo.Title(mainAssembly) ?? mainAssembly.GetName().Name!;

        var registryLocation = !string.IsNullOrEmpty(appCompany)
                                   ? $@"Software\{appCompany}\{AppTitle}\AutoUpdater"
                                   : $@"Software\{AppTitle}\AutoUpdater";

        PersistenceProvider = new Registry(registryLocation);

        UpdateInfoEventArgs? args;

        BaseUri = new Uri(AppCastURL);
        using var response = await GetWebClient(BaseUri, BasicAuthHeaderValue);
        var xml = await response.Content.ReadAsStringAsync();

        if (ParseUpdateInfoEvent == null)
        {
            if (string.IsNullOrEmpty(xml))
                throw new Exception("It is required to handle ParseUpdateInfoEvent when XML url is not specified.");

            var xmlSerializer = new XmlSerializer(typeof(UpdateInfoEventArgs));
            var xmlTextReader = new XmlTextReader(new StringReader(xml)) { XmlResolver = null };
            args = (UpdateInfoEventArgs) xmlSerializer.Deserialize(xmlTextReader)!;
        }
        else
        {
            if (xml is null)
                throw new NullReferenceException();

            var parseArgs = new ParseUpdateInfoEventArgs(this, xml);
            ParseUpdateInfoEvent(parseArgs);
            args = parseArgs.UpdateInfo;
        }

        if (string.IsNullOrEmpty(args?.CurrentVersion) || string.IsNullOrEmpty(args.DownloadURL))
            throw new MissingFieldException();

        args.InstalledVersion = InstalledVersion ?? mainAssembly.GetName().Version!;
        args.IsUpdateAvailable = new Version(args.CurrentVersion) > args.InstalledVersion;

        if (!Mandatory)
        {
            if (string.IsNullOrEmpty(args.Mandatory.MinimumVersion) || args.InstalledVersion < new Version(args.Mandatory.MinimumVersion))
            {
                Mandatory = args.Mandatory.Value;
                UpdateMode = args.Mandatory.UpdateMode;
            }
        }

        if (Mandatory)
        {
            ShowRemindLaterButton = false;
            ShowSkipButton = false;
        }
        else
        {
            // Read the persisted state from the persistence provider.
            // This method makes the persistence handling independent from the storage method.
            var skippedVersion = PersistenceProvider.GetSkippedVersion();
            if (skippedVersion != null)
            {
                var currentVersion = new Version(args.CurrentVersion);
                if (currentVersion <= skippedVersion)
                    return null;

                if (currentVersion > skippedVersion)
                    // Update the persisted state. Its no longer makes sense to have this flag set as we are working on a newer application version.
                    PersistenceProvider.SetSkippedVersion(null);
            }

            var remindLaterAt = PersistenceProvider.GetRemindLater();
            if (remindLaterAt == null)
                return args;

            if (DateTime.Compare(DateTime.Now, remindLaterAt.Value) < 0)
                args.TimeStamp = remindLaterAt.Value;
        }

        return args;
    }


    private bool StartUpdate(object? result)
    {
        if (result is DateTime time)
            SetTimer(time);
        else
        {
            if (result is not UpdateInfoEventArgs args)
                return false;

            if (CheckForUpdateEvent != null)
                CheckForUpdateEvent(args);
            else
            {
                if (args.IsUpdateAvailable)
                {
                    if (Mandatory && UpdateMode == Mode.ForcedDownload)
                    {
                        DownloadUpdate(args);
                        Exit();
                    }
                    else
                        ShowUpdateForm(args);

                    return true;
                }

                if (ReportErrors)
                {
                    MessageBox.Show(Environment.GetEnvironmentVariable("UpdateUnavailableMessage")!,
                                    Environment.GetEnvironmentVariable("UpdateUnavailableCaption")!,
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        return false;
    }

    private void ShowError(Exception exception)
    {
        if (CheckForUpdateEvent != null)
            CheckForUpdateEvent(
                new UpdateInfoEventArgs
                {
                    Error = exception,
                    Owner = this
                });
        else
        {
            if (ReportErrors)
            {
                if (exception is WebException)
                {
                    MessageBox.Show(
                        Environment.GetEnvironmentVariable("UpdateCheckFailedMessage")!,
                        Environment.GetEnvironmentVariable("UpdateCheckFailedCaption")!,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(
                        exception.Message,
                        exception.GetType().ToString(),
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        Running = false;
    }

    /// <summary>
    ///     Detects and exits all instances of running assembly, including current.
    /// </summary>
    internal void Exit()
    {
        var currentProcess = Process.GetCurrentProcess();
        foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
        {
            try
            {
                var processPath = process.MainModule?.FileName;

                // Get all instances of assembly except current
                if (process.Id == currentProcess.Id || currentProcess.MainModule?.FileName != processPath)
                    continue;

                if (process.CloseMainWindow())
                {
                    process.WaitForExit((int)TimeSpan.FromSeconds(10)
                        .TotalMilliseconds); // Give some time to process message
                }

                if (process.HasExited)
                    continue;

                process.Kill(); //TODO: Show UI message asking user to close program himself instead of silently killing it
            }
            catch (Exception)
            {
                // ignored
            }
        }

        if (ApplicationExitEvent != null)
            ApplicationExitEvent();
        else
        {
            if (Dispatcher is not null)
                Dispatcher.Invoke(Application.Current!.Shutdown);
            else
                Environment.Exit(0);
        }
    }


    internal string GetUserAgent() => string.IsNullOrEmpty(HttpUserAgent) ? "AutoUpdater.NET" : HttpUserAgent;

    internal void SetTimer(DateTime remindLater)
    {
        var timeSpan = remindLater - DateTime.Now;

        var context = SynchronizationContext.Current;

        _remindLaterTimer = new Timer
        {
            Interval = Math.Max(1, timeSpan.TotalMilliseconds),
            AutoReset = false
        };

        _remindLaterTimer.Elapsed += delegate
        {
            _remindLaterTimer = null;
            if (context != null)
            {
                try
                {
                    context.Send(_ => Start(), null);
                }
                catch (InvalidAsynchronousStateException)
                {
                    Start();
                }
            }
            else
            {
                Start();
            }
        };

        _remindLaterTimer.Start();
    }


    /// <summary>
    ///     Opens the Download window that download the update and execute the installer when download completes.
    /// </summary>
    public static void DownloadUpdate(UpdateInfoEventArgs args)
    {
        //var downloadDialog = new DownloadUpdateDialog(args);
        //return downloadDialog.ShowDialog();
    }


    /// <summary>
    ///     Shows standard update dialog.
    /// </summary>
    public void ShowUpdateForm(UpdateInfoEventArgs args)
    {
        //var updateForm = new UpdateForm(args);
        //updateForm.Closed += (_, _) => Exit();
        //Task.Run(updateForm.ShowDialog).ConfigureAwait(false);
    }


    internal Task<HttpResponseMessage> GetWebClient(Uri uri, AuthenticationHeaderValue basicAuthentication)
    {
        BaseUri = uri;
        HttpWebClient.DefaultRequestHeaders.Authorization = basicAuthentication;
        return HttpWebClient.GetAsync(BaseUri);
    }
}