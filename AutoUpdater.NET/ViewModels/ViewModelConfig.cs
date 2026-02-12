// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Modifiers;
using AutoUpdaterDotNET.TypeResolvers;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Windows.Threading;
using WindowService.ViewModels;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig : ViewModelRestricted, IViewModelConfig
{
    #region Static Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public static HttpClient HttpWebClient => SingletonHttpClient.Value;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Static Properties


    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    // Shadow copy
    private readonly Config               _config, _configOrig;
    private readonly DispatcherTimer      _updateTimer        = new();
    private static   Lazy<HttpClient>     SingletonHttpClient = null!;
    private          System.Timers.Timer? _remindLaterTimer;
    private          Assembly             _assembly = null!;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public IConfig Config => _config;


    [ObservableProperty]
    public partial IEnumerable<RemindLaterFormat> RemindLaterFormatEnumValues { get; set; } = Enum.GetValues<RemindLaterFormat>().Skip(1);


    private Uri _baseUri
    {
        get;
        set
        {
            field = value;

            NetworkCredential? credentials;
            if (field.Scheme.Equals(Uri.UriSchemeFtp))
                credentials = FtpCredentials ?? throw new NullReferenceException($"{nameof(FtpCredentials)} must not be null for file transfer protocol.");
            else
                credentials = CredentialCache.DefaultCredentials as NetworkCredential;

            var httpClientHandler = new HttpClientHandler
            {
                Credentials             = credentials,
                PreAuthenticate         = true,
                AllowAutoRedirect       = true,
                MaxConnectionsPerServer = 1,
                UseCookies              = false,
                AutomaticDecompression  = DecompressionMethods.GZip,
                UseDefaultCredentials   = true,
                UseProxy                = _config.ProxyEnabled,
                Proxy                   = _config.ProxyEnabled ? new WebProxy { Address = new Uri(_config.ProxyUri ?? throw new NullReferenceException(nameof(_config.ProxyUri))) } : null,
                DefaultProxyCredentials = _config.ProxyEnabled ? new NetworkCredential(_config.ProxyUserName, _config.ProxyPassword) : new CredentialCache()
            };
            SingletonHttpClient = new(() => new(httpClientHandler)
            {
                BaseAddress = value,
                DefaultRequestHeaders = {
                    Authorization = BasicAuthHeaderValue
                }
            });
        }
    } = null!;

    internal bool Running { get; set; }

    /// <summary>
    ///     Set Basic Authentication credentials required to download the XML file.
    /// </summary>
    public AuthenticationHeaderValue? BasicAuthHeaderValue { get; set; }

    /// <summary>
    ///     Login/password/domain for FTP-request
    /// </summary>
    public NetworkCredential? FtpCredentials { get; set; }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public event Action?                      UpdateComplete;
    public event Action<UpdateInfoEventArgs>? CheckForUpdates;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events



    /// <summary>
    ///     Default Constructor
    /// </summary>
    public ViewModelConfig(BaseServiceDependencies dependencies) : base(dependencies)
    {
        _config = new()
        {
            EqualsPredicate = new Lazy<Func<bool>>(() => _configOrig?.Equals(_config) ?? true)
        };
        _configOrig = new(_config);
    }


    private static readonly JsonSerializerOptions _jso = new()
    {
        WriteIndented       = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        TypeInfoResolver    = new DependancyPropertyTypeResolver<Config>
        {
            Modifiers = { Modifier.AlphabetizeProperties }
        }
    };
}