// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     NetworkCredential2.cs
// Author:   Latency McLaughlin
// Date:     02/23/2026
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Net;
using System.Security;

namespace AutoUpdaterDotNET.Models;

public partial class NetworkCredential2 : ObservableObject
{
    private readonly NetworkCredential _networkCredential = new();


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public NetworkCredential2()
    { }


    /// <summary>
    ///     Copy Constructor
    /// </summary>
    /// <param name="credential"></param>
    public NetworkCredential2(NetworkCredential credential) : this()
    {
        UserName       = credential.UserName;
        Password       = credential.Password;
        ShowPassword   = true;
        SecurePassword = credential.SecurePassword;
        Domain         = credential.Domain;
    }

    public static explicit operator NetworkCredential2(NetworkCredential profile) => new(profile);
    public static implicit operator NetworkCredential(NetworkCredential2 profile) => profile._networkCredential;


    [ObservableProperty]
    [DisplayName("User Name")]
    [Description("The user name associated with this credential.")]
    public partial string UserName { get; set; } = string.Empty;
    partial void OnUserNameChanged(string value) => _networkCredential.UserName = value;


    [ObservableProperty]
    [Description("The password for the user name.")]
    public partial string Password { get; set; } = string.Empty;
    partial void OnPasswordChanged(string value) => _networkCredential.Password = value;


    [ObservableProperty]
    [Description("Show the secure password.")]
    public partial bool ShowPassword { get; set; }


    [ObservableProperty]
    [Description("The secure password for the user name.")]
    public partial SecureString SecurePassword { get; set; } = new();
    partial void OnSecurePasswordChanged(SecureString value) => _networkCredential.SecurePassword = value;


    [ObservableProperty]
    [Description("The machine name that verifies the credentials. Usually this is the host machine.")]
    public partial string Domain { get; set; } = string.Empty;
    partial void OnDomainChanged(string value) => _networkCredential.Domain = value;
}