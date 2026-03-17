// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     WindowSize.cs
// Author:   Latency McLaughlin
// Date:     06/29/2025
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows;

namespace AutoUpdaterDotNET.Models;

public partial class WindowSize : ObservableObject
{
    /// <summary>
    ///     Default Constructor
    /// </summary>
    public WindowSize() { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <returns></returns>
    public WindowSize(WindowSize? size) : this()
    {
        if (size is null)
            return;

        Height = size.Height;
        Width  = size.Width;
    }


    /// <summary>
    ///     Copy Constructor (Overload +2)
    /// </summary>
    /// <param name="size"></param>
    public WindowSize(Size size) : this()
    {
        Height = size.Height;
        Width  = size.Width;
    }


    public static explicit operator WindowSize(Size v) => new(v);
    public static implicit operator Size(WindowSize s) => new(s.Width, s.Height);



    [ObservableProperty]
    [Category("Window Size")]
    [Description("The height of the window.")]
    public partial double Height { get; set; }


    [ObservableProperty]
    [Category("Window Size")]
    [Description("The width of the window.")]
    public partial double Width { get; set; }

    public override string ToString() => $"Width={Width}, Height={Height}";
}