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
    private static HttpClientHandler HttpClientHandlerInstance => SingletonHttpClientHandler.Value;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Static Properties


    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    // Shadow copy
    private readonly ViewModelMainConfig _configOrig;
    private readonly DispatcherTimer     _updateTimer = new();
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

    private System.Timers.Timer? _remindLaterTimer;
    private Assembly?            _assembly;
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


    private HttpClient HttpWebClient { get; } = new(HttpClientHandlerInstance);

    internal Uri _baseUri
    {
        get;
        set
        {
            field                                 = value;
            HttpClientHandlerInstance.Credentials = field.Scheme.Equals(Uri.UriSchemeFtp) ? FtpCredentials : CredentialCache.DefaultCredentials;
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

    /// <summary>
    ///     Set this to an instance implementing the IPersistenceProvider interface for using a data storage method different
    ///     from the default Windows Registry based one.
    /// </summary>
    public IPersistenceProvider? PersistenceProvider { get; set; }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=+
    public event Action?                           UpdateComplete;
    public event Action<UpdateInfoEventArgs>?      CheckForUpdates;
    public event Action<ParseUpdateInfoEventArgs>? ParseUpdateInfo;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events
}