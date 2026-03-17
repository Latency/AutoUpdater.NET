// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     HttpClient2.cs
// Author:   Latency McLaughlin
// Date:     03/06/2026
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text.Json.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AutoUpdaterDotNET.Models;

public partial class HttpClient2 : ObservableObject
{
    #region Fields

    private readonly HttpClient _httpClient;

    #endregion Fields


    #region Constructors

    /// <summary>
    ///     Default Constructor
    /// </summary>
    public HttpClient2() : this(new HttpClientHandler())
    {
    }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="handler"></param>
    public HttpClient2(HttpMessageHandler handler) : this(handler, true)
    {
    }


    /// <summary>
    ///     Copy Constructor (Overload +2)
    /// </summary>
    /// <param name="profile"></param>
    public HttpClient2(HttpClient2? profile) : this()
    {
        if (profile is null)
            return;

        BaseAddress                  = profile.BaseAddress;
        DefaultRequestHeaders        = profile.DefaultRequestHeaders;
        DefaultRequestVersion        = profile.DefaultRequestVersion;
        DefaultVersionPolicy         = profile.DefaultVersionPolicy;
        MaxResponseContentBufferSize = profile.MaxResponseContentBufferSize;
        Timeout                      = profile.Timeout;
    }


    /// <summary>
    ///     Copy Constructor (Overload +3)
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="disposeHandler"></param>
    public HttpClient2(HttpMessageHandler handler, bool disposeHandler) : this(new HttpClient(handler, disposeHandler))
    {
    }


    /// <summary>
    ///     Copy Constructor (Overload +4)
    /// </summary>
    /// <param name="client"></param>
    public HttpClient2(HttpClient client)
    {
        _httpClient           = client;
        DefaultRequestHeaders = client.DefaultRequestHeaders;
        DefaultRequestVersion = new Version2(_httpClient.DefaultRequestVersion);
    }

    #endregion Constructors


    public static explicit operator HttpClient2(HttpClient client) => new(client);
    public static implicit operator HttpClient(HttpClient2 client) => client._httpClient;


    #region Properties

    public static IWebProxy DefaultProxy
    {
        get => HttpClient.DefaultProxy;
        set => HttpClient.DefaultProxy = value;
    }


    [ObservableProperty]
    [ReadOnly(true)]
    [Browsable(false)]
    [ExpandableObject]
    [JsonIgnore]
    [Description("Should be sent with each request.")]
    public partial HttpRequestHeaders DefaultRequestHeaders { get; set; }
    partial void OnDefaultRequestHeadersChanged(HttpRequestHeaders value)
    {
        var fld = _httpClient.GetType().GetField("_defaultRequestHeaders", BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic);
        fld!.SetValue(_httpClient, value, BindingFlags.Instance | BindingFlags.SetField | BindingFlags.NonPublic, null, CultureInfo.CurrentCulture);
    }


    [ObservableProperty]
    [DefaultValue("1.1.0.0")]
    [ExpandableObject]
    [Description("The default HTTP version used on subsequent requests made by this HttpClient instance.")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public partial Version2 DefaultRequestVersion { get; set; }
    partial void OnDefaultRequestVersionChanged(Version2 value) => _httpClient.DefaultRequestVersion = value;


    /// <summary>
    /// Gets or sets the default value of <see cref="HttpRequestMessage.VersionPolicy" /> for implicitly created requests in convenience methods,
    /// e.g.: <see cref="GetAsync(string?)" />, <see cref="PostAsync(string?, HttpContent)" />.
    /// </summary>
    /// <remarks>
    /// Note that this property has no effect on any of the <see cref="Send(HttpRequestMessage)" /> and <see cref="SendAsync(HttpRequestMessage)" /> overloads
    /// since they accept fully initialized <see cref="HttpRequestMessage" />.
    /// </remarks>
    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Description("The default version policy for implicitly created requests in convenience methods.")]
    public partial HttpVersionPolicy DefaultVersionPolicy { get; set; }
    partial void OnDefaultVersionPolicyChanged(HttpVersionPolicy value) => _httpClient.DefaultVersionPolicy = value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [DisplayName("Base Address")]
    [Description("The base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.")]
    public partial Uri? BaseAddress { get; set; }
    partial void OnBaseAddressChanged(Uri? value) => _httpClient.BaseAddress = value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Description("Defines the maximum duration an application will wait for an HTTP request to complete before terminating the request and raising an exception.")]
    public partial TimeSpan Timeout { get; set; }
    partial void OnTimeoutChanged(TimeSpan value) => _httpClient.Timeout = value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [DefaultValue(2000000000)]
    [DisplayName("Max Response Content Buffer Size")]
    [Description("Controls the maximum number of bytes to buffer when reading the response content.")]
    public partial ulong MaxResponseContentBufferSize { get; set; } = 2000000000;
    partial void OnMaxResponseContentBufferSizeChanged(ulong value) => _httpClient.MaxResponseContentBufferSize = (long) value;

    #endregion Properties


    #region Methods

    #region Simple Get Overloads

    public Task<string> GetStringAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) => _httpClient.GetStringAsync(requestUri);

    public Task<string> GetStringAsync(Uri? requestUri) => _httpClient.GetStringAsync(requestUri);

    public Task<string> GetStringAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken) => _httpClient.GetStringAsync(requestUri, cancellationToken);

    public Task<string> GetStringAsync(Uri? requestUri, CancellationToken cancellationToken) => _httpClient.GetStringAsync(requestUri, cancellationToken);


    public Task<byte[]> GetByteArrayAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) => _httpClient.GetByteArrayAsync(requestUri);

    public Task<byte[]> GetByteArrayAsync(Uri? requestUri) => _httpClient.GetByteArrayAsync(requestUri);

    public Task<byte[]> GetByteArrayAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken) => _httpClient.GetByteArrayAsync(requestUri, cancellationToken);

    public Task<byte[]> GetByteArrayAsync(Uri? requestUri, CancellationToken cancellationToken) => _httpClient.GetByteArrayAsync(requestUri, cancellationToken);


    public Task<Stream> GetStreamAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) => _httpClient.GetStreamAsync(requestUri);

    public Task<Stream> GetStreamAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken) => _httpClient.GetStreamAsync(requestUri, cancellationToken);

    public Task<Stream> GetStreamAsync(Uri? requestUri) => _httpClient.GetStreamAsync(requestUri);

    public Task<Stream> GetStreamAsync(Uri? requestUri, CancellationToken cancellationToken) => _httpClient.GetStreamAsync(requestUri, cancellationToken);

    #endregion Simple Get Overloads


    #region REST Send Overloads

    public Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) => _httpClient.GetAsync(requestUri);

    public Task<HttpResponseMessage> GetAsync(Uri? requestUri) => _httpClient.GetAsync(requestUri);

    public Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpCompletionOption completionOption) => _httpClient.GetAsync(requestUri, completionOption);

    public Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption) => _httpClient.GetAsync(requestUri, completionOption);

    public Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken) => _httpClient.GetAsync(requestUri, cancellationToken);

    public Task<HttpResponseMessage> GetAsync(Uri? requestUri, CancellationToken cancellationToken) => _httpClient.GetAsync(requestUri, cancellationToken);

    public Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => _httpClient.GetAsync(requestUri, completionOption, cancellationToken);

    public Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => _httpClient.GetAsync(requestUri, completionOption, cancellationToken);

    public Task<HttpResponseMessage> PostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) => _httpClient.PostAsync(requestUri, content);

    public Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content) => _httpClient.PostAsync(requestUri, content);

    public Task<HttpResponseMessage> PostAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content, CancellationToken cancellationToken) => _httpClient.PostAsync(requestUri, content, cancellationToken);

    public Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) => _httpClient.PostAsync(requestUri, content, cancellationToken);

    public Task<HttpResponseMessage> PutAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) => _httpClient.PutAsync(requestUri, content);

    public Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content) => _httpClient.PutAsync(requestUri, content);

    public Task<HttpResponseMessage> PutAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content, CancellationToken cancellationToken) => _httpClient.PutAsync(requestUri, content, cancellationToken);

    public Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) => _httpClient.PutAsync(requestUri, content, cancellationToken);

    public Task<HttpResponseMessage> PatchAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content) => _httpClient.PatchAsync(requestUri, content);

    public Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content) => _httpClient.PatchAsync(requestUri, content);

    public Task<HttpResponseMessage> PatchAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, HttpContent? content, CancellationToken cancellationToken) => _httpClient.PatchAsync(requestUri, content, cancellationToken);

    public Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) => _httpClient.PatchAsync(requestUri, content, cancellationToken);

    public Task<HttpResponseMessage> DeleteAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri) => _httpClient.DeleteAsync(requestUri);

    public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri) => _httpClient.DeleteAsync(requestUri);

    public Task<HttpResponseMessage> DeleteAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri, CancellationToken cancellationToken) => _httpClient.DeleteAsync(requestUri, cancellationToken);

    public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri, CancellationToken cancellationToken) => _httpClient.DeleteAsync(requestUri, cancellationToken);

    #endregion REST Send Overloads


    #region Advanced Send Overloads

    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request) => _httpClient.Send(request);

    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption) => _httpClient.Send(request, completionOption);

    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken) => _httpClient.Send(request, cancellationToken);

    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) => _httpClient.Send(request, completionOption, cancellationToken);

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => _httpClient.SendAsync(request);

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => _httpClient.SendAsync(request, cancellationToken);

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption) => _httpClient.SendAsync(request, completionOption);

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) => _httpClient.SendAsync(request, completionOption, cancellationToken);

    public void CancelPendingRequests() => _httpClient.CancelPendingRequests();

    #endregion Advanced Send Overloads

    #endregion Methods
}