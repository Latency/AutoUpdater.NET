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
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public partial class NetworkCredential2 : ObservableObject, IEquatable<NetworkCredential2>
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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [DisplayName("User Name")]
    [Description("The user name associated with this credential.")]
    public partial string UserName { get; set; }
    partial void OnUserNameChanged(string? value) => _networkCredential.UserName = value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Description("The password for the user name.")]
    public partial string Password { get; set; }
    partial void OnPasswordChanged(string? value) => _networkCredential.Password = value;


    [ObservableProperty]
    [DisplayName("Show Password")]
    [DefaultValue(true)]
    [Description("Show the secure password.")]
    public partial bool ShowPassword { get; set; }


    [ObservableProperty]
    [Browsable(false)]
    [JsonIgnore]
    [DisplayName("Secure Password")]
    [Description("The secure password for the user name.")]
    public partial SecureString SecurePassword { get; set; } = new();
    partial void OnSecurePasswordChanged(SecureString value) => _networkCredential.SecurePassword = value;


    [ObservableProperty]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Description("The machine name that verifies the credentials. Usually this is the host machine.")]
    public partial string? Domain { get; set; }
    partial void OnDomainChanged(string? value) => _networkCredential.Domain = value;


    public bool Equals(NetworkCredential2? other) => other is not null                  &&
                                                     Domain       == other.Domain       &&
                                                     ShowPassword == other.ShowPassword &&
                                                     UserName     == other.UserName     &&
                                                     Password     == other.Password;


    public override bool Equals(object? obj) => Equals(obj as NetworkCredential2);


    public override int GetHashCode() => HashCode.Combine(Domain, ShowPassword, UserName);


    public bool IsDefault() => (from prop in GetType().GetProperties()
                                where prop.Name != "SecurePassword"
                                let dflt = prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null
                                let v1 = prop.GetValue(this)
                                select v1 switch
                                {
                                    null => dflt == null,
                                    _    => v1.Equals(dflt)
                                }).Aggregate(true, (current, r1) => current & r1);
}