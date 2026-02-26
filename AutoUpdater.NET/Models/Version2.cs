// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Version2.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.Models;

public partial class Version2 : ObservableObject
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public Version2() { }

    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="obj"></param>
    public Version2(Version2 obj)
    {
        Major    = obj.Major;
        Minor    = obj.Minor;
        Build    = obj.Build;
        Revision = obj.Revision;
    }

    /// <summary>
    ///     Copy Constructor (Overload +2)
    /// </summary>
    /// <param name="v"></param>
    public Version2(Version v) : this((uint) v.Major, (uint) v.Minor, (uint) v.Build, (uint) v.Revision) { }

    /// <summary>
    ///     Copy Constructor (Overload +3)
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    /// <param name="build"></param>
    /// <param name="revision"></param>
    public Version2(uint major, uint minor, uint build, uint revision) : this()
    {
        Major    = major;
        Minor    = minor;
        Build    = build;
        Revision = revision;
    }

    /// <summary>
    ///     Copy Constructor (Overload +4)
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    /// <param name="build"></param>
    public Version2(uint major, uint minor, uint build) : this(major, minor, build, 0) { }

    /// <summary>
    ///     Copy Constructor (Overload +5)
    /// </summary>
    /// <param name="major"></param>
    /// <param name="minor"></param>
    public Version2(uint major, uint minor) : this(major, minor, 0, 0) { }

    /// <summary>
    ///     Copy Constructor (Overload +6)
    /// </summary>
    /// <param name="version"></param>
    public Version2(string version) : this(new Version(version)) { }


    public static explicit operator Version2(Version v) => new(v);
    public static implicit operator Version(Version2 v) => new((int) v.Major, (int) v.Minor, (int) v.Build, (int) v.Revision);



    [ObservableProperty]
    public partial uint Major { get; set; }


    [ObservableProperty]
    public partial uint Minor { get; set; }


    [ObservableProperty]
    public partial uint Build { get; set; }


    [ObservableProperty]
    public partial uint Revision { get; set; }


    public override string ToString() => $"{Major}.{Minor}.{Build}.{Revision}";
}