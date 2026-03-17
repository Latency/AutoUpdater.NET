// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Version2.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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
    public Version2(Version2? obj)
    {
        if (obj is null)
            return;

        Major    = obj.Major;
        Minor    = obj.Minor;
        Build    = obj.Build;
        Revision = obj.Revision;
    }

    /// <summary>
    ///     Copy Constructor (Overload +2)
    /// </summary>
    /// <param name="v"></param>
    public Version2(Version v) : this(v.Major >= 0 ? (uint) v.Major : 0, v.Minor >= 0 ? (uint) v.Minor : 0, v.Build >= 0 ? (uint) v.Build : 0, v.Revision >= 0 ? (uint) v.Revision : 0) { }

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


    [PropertyOrder(1)]
    [ObservableProperty]
    public partial uint Major { get; set; }


    [PropertyOrder(2)]
    [ObservableProperty]
    public partial uint Minor { get; set; }


    [PropertyOrder(3)]
    [ObservableProperty]
    public partial uint Revision { get; set; }


    [PropertyOrder(4)]
    [ObservableProperty]
    public partial uint Build { get; set; }


    public override string ToString() => $"{Major}.{Minor}.{Build}.{Revision}";
}