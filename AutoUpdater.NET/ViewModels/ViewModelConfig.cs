// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Modifiers;
using AutoUpdaterDotNET.TypeResolvers;
using System.Text.Json;
using System.Windows.Threading;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig : ViewModelMainConfig, IViewModelConfig
{
    #region Static Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public static HttpClient HttpWebClient => SingletonHttpClient.Value;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Static Properties


    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    // Shadow copy
    private readonly ViewModelMainConfig     _configOrig;
    private readonly DispatcherTimer         _updateTimer               = new();
    private static   Lazy<HttpClient>        SingletonHttpClient        = null!;

    private          System.Timers.Timer?    _remindLaterTimer;
    private          Assembly                _assembly = null!;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    /// <summary>
    ///     Default Constructor
    /// </summary>
    /// <param name="serviceProvider"></param>
    public ViewModelConfig(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _configOrig = new ViewModelMainConfig(this, serviceProvider);
    }


    private static readonly JsonSerializerOptions _jso = new()
    {
        WriteIndented       = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        TypeInfoResolver    = new DependancyPropertyTypeResolver<ViewModelMainConfig>
        {
            Modifiers = { Modifier.AlphabetizeProperties }
        }
    };


    public override bool Equals() => _configOrig.Equals(this);


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

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
                UseProxy                = ProxyEnabled,
                Proxy                   = ProxyEnabled ? new WebProxy { Address = new Uri(ProxyUri ?? throw new NullReferenceException(nameof(ProxyUri))) } : null,
                DefaultProxyCredentials = ProxyEnabled ? new NetworkCredential(ProxyUserName, ProxyPassword) : new CredentialCache()
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
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=+
    public event Action?                      UpdateComplete;
    public event Action<UpdateInfoEventArgs>? CheckForUpdates;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events
}