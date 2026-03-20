// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     FtpProfile2.cs
// Author:   Latency McLaughlin
// Date:     02/23/2026
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using FluentFTP;
using System.ComponentModel;
using System.Security.Authentication;
using System.Text;
using System.Text.Json.Serialization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace AutoUpdaterDotNET.Models;

public partial class FtpProfile2 : ObservableObject
{
    private readonly FtpProfile _ftpProfile = new();


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public FtpProfile2()
    { }


    /// <summary>
    ///     Copy Constructor
    /// </summary>
    /// <param name="profile"></param>
    public FtpProfile2(FtpProfile2? profile) : this()
    {
        if (profile is null)
            return;

        Host               = profile.Host;
        Credentials        = profile.Credentials;
        Encryption         = profile.Encryption;
        Protocols          = profile.Protocols;
        DataConnection     = profile.DataConnection;
        Encoding           = profile.Encoding;
        Timeout            = profile.Timeout;
        SocketPollInterval = profile.SocketPollInterval;
        RetryAttempts      = profile.RetryAttempts;
        EncodingVerified   = profile.EncodingVerified;
    }


    /// <summary>
    ///     Copy Constructor
    /// </summary>
    /// <param name="profile"></param>
    public FtpProfile2(FtpProfile profile) : this()
    {
        Host               = profile.Host;
        Credentials        = (NetworkCredential2) profile.Credentials;
        Encryption         = profile.Encryption;
        Protocols          = profile.Protocols;
        DataConnection     = profile.DataConnection;
        Encoding           = (Encoding2) (profile.Encoding ?? new UTF8Encoding());
        Timeout            = (uint) profile.Timeout;
        SocketPollInterval = (uint) profile.SocketPollInterval;
        RetryAttempts      = (uint) profile.RetryAttempts;
        EncodingVerified   = profile.EncodingVerified;
    }

    public static explicit operator FtpProfile2(FtpProfile profile) => new(profile);
    public static implicit operator FtpProfile(FtpProfile2 profile) => profile._ftpProfile;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Description("The host IP address or URL of the FTP server")]
    public partial string? Host { get; set; }
    partial void OnHostChanged(string? value) => _ftpProfile.Host = value ?? string.Empty;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ExpandableObject]
    [Description("The FTP username and password used to login")]
    public partial NetworkCredential2? Credentials { get; set; }
    partial void OnCredentialsChanged(NetworkCredential2? value) => _ftpProfile.Credentials = value ?? new();


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Description("A working Encryption Mode found for this profile")]
    public partial FtpEncryptionMode Encryption { get; set; }
    partial void OnEncryptionChanged(FtpEncryptionMode value) => _ftpProfile.Encryption = value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Description("A working SSL Protocol setting found for this profile")]
    public partial SslProtocols Protocols { get; set; }
    partial void OnProtocolsChanged(SslProtocols value) => _ftpProfile.Protocols = value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [DisplayName("Data Connection")]
    [Description("A working Data Connection Type found for this profile")]
    public partial FtpDataConnectionType DataConnection { get; set; }
    partial void OnDataConnectionChanged(FtpDataConnectionType value) => _ftpProfile.DataConnection = value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [ExpandableObject]
    [DefaultValue(typeof(Encoding2), "utf-8")]
    [Description("A working Encoding setting found for this profile")]
    public partial Encoding2? Encoding { get; set; }
    partial void OnEncodingChanged(Encoding2? value) => _ftpProfile.Encoding = value ?? (Encoding2) new UTF8Encoding();


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Description("A working Timeout setting found for this profile, or 0 if default value should be used")]
    public partial uint Timeout { get; set; }
    partial void OnTimeoutChanged(uint value) => _ftpProfile.Timeout = (int) value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [DisplayName("Socket Poll Interval")]
    [Description("A working SocketPollInterval setting found for this profile, or 0 if default value should be used")]
    public partial uint SocketPollInterval { get; set; }
    partial void OnSocketPollIntervalChanged(uint value) => _ftpProfile.SocketPollInterval = (int) value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [DisplayName("Retry Attempts")]
    [Description("A working RetryAttempts setting found for this profile, or 0 if default value should be used")]
    public partial uint RetryAttempts { get; set; }
    partial void OnRetryAttemptsChanged(uint value) => _ftpProfile.RetryAttempts = (int) value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Category("Encoding")]
    [DisplayName("Encoding Verified")]
    [Description("If the server surely supports the given encoding")]
    public partial bool EncodingVerified { get; set; }
    partial void OnEncodingVerifiedChanged(bool value) => _ftpProfile.EncodingVerified = value;
}