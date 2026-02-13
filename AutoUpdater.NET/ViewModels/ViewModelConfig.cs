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
using FluentFTP;
using WindowService.ViewModels;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig : ViewModelRestricted, IViewModelConfig
{
    #region Static Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public static HttpClient HttpWebClient => SingletonHttpClient.Value;
    public static AsyncFtpClient FtpClient => SingletonFtpClient.Value;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Static Properties


    #region Static Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private static Lazy<HttpClient>     SingletonHttpClient = null!;
    private static Lazy<AsyncFtpClient> SingletonFtpClient  = null!;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Static Fields


    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    // Shadow copy
    private readonly Config               _config, _configOrig;
    private readonly DispatcherTimer      _updateTimer        = new();
    private          System.Timers.Timer? _remindLaterTimer;
    private          Assembly             _assembly = null!;

    private          CancellationTokenSource? _ftpCTS;
    private readonly Progress<FtpProgress>    _ftpProgress = new();
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

            var httpClientHandler = new HttpClientHandler
            {
                Credentials             = CredentialCache.DefaultCredentials as NetworkCredential,
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
            SingletonFtpClient = new(() =>
            {
                if (_config.BasicAuthUserName is null)
                    throw new NullReferenceException(_config.BasicAuthUserName);

                var client = new AsyncFtpClient(value.Host, _config.BasicAuthUserName, _config.BasicAuthPassword ?? string.Empty);

                // Recommended: Auto-detect encryption and accept any server certificate for simplicity
                client.Config!.EncryptionMode         = FtpEncryptionMode.Auto;
                client.Config!.ValidateAnyCertificate = true;

                return client;
            });
        }
    } = null!;

    internal bool Running { get; set; }

    /// <summary>
    ///     Set Basic Authentication credentials required to download the XML file.
    /// </summary>
    public AuthenticationHeaderValue? BasicAuthHeaderValue { get; set; }

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