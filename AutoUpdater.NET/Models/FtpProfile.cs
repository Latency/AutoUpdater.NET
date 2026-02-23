// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     FtpProfile.cs
// Author:   Latency McLaughlin
// Date:     02/23/2026
// ****************************************************************************

using FluentFTP;
using System.ComponentModel;
using System.Net;
using System.Security.Authentication;
using System.Text;

namespace AutoUpdaterDotNET.Models;

public class FtpProfile2 : FtpProfile
{
    [Description("The host IP address or URL of the FTP server")]
    public new string? Host
    {
        get => base.Host;
        set => base.Host = value!;
    }

    [Description("The FTP username and password used to login")]
    public new NetworkCredential? Credentials
    {
        get => base.Credentials;
        set => base.Credentials = value!;
    }

    [Description("A working Encryption Mode found for this profile")]
    public new FtpEncryptionMode Encryption
    {
        get => base.Encryption;
        set => base.Encryption = value;
    }

    [Description("A working SSL Protocol setting found for this profile")]
    public new SslProtocols Protocols
    {
        get => base.Protocols;
        set => base.Protocols = value;
    }

    [Description("A working Data Connection Type found for this profile")]
    public new FtpDataConnectionType DataConnection
    {
        get => base.DataConnection;
        set => base.DataConnection = value;
    }

    [Description("A working Encoding setting found for this profile")]
    public new Encoding? Encoding
    {
        get => base.Encoding;
        set => base.Encoding = value!;
    }

    [Description("A working Timeout setting found for this profile, or 0 if default value should be used")]
    public new uint Timeout
    {
        get => (uint) base.Timeout;
        set => base.Timeout = (int) value;
    }

    [Description("A working SocketPollInterval setting found for this profile, or 0 if default value should be used")]
    public new uint SocketPollInterval
    {
        get => (uint) base.SocketPollInterval;
        set => base.SocketPollInterval = (int) value;
    }

    [Description("A working RetryAttempts setting found for this profile, or 0 if default value should be used")]
    public new uint RetryAttempts
    {
        get => (uint) base.RetryAttempts;
        set => base.RetryAttempts = (int) value;
    }

    [Description("If the server surely supports the given encoding")]
    public new bool EncodingVerified
    {
        get => base.EncodingVerified;
        set => base.EncodingVerified = value;
    }
}