// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     RestrictedWindow.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Windows;
using System.Windows.Interop;
using AutoUpdaterDotNET.Interops;

namespace AutoUpdaterDotNET.Controls;

public abstract class RestrictedWindow : Window
{
    // Handle to current window.
    private nint _hWnd;


    /// <summary>
    ///     Default Constructor
    /// </summary>
    protected RestrictedWindow()
    {
        ControlBox            = false;
        ResizeMode            = ResizeMode.NoResize;
        ShowActivated         = true;
        ShowInTaskbar         = true;
        Topmost               = true;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        WindowStyle           = WindowStyle.SingleBorderWindow;

        Loaded += Window_OnLoaded;
    }


    public bool ControlBox
    {
        get;
        set
        {
            if (field.Equals(value))
                return;

            field = value;
            ToggleControlBox(field);
        }
    }


    private void Window_OnLoaded(object sender, RoutedEventArgs e)
    {
        _hWnd = new WindowInteropHelper(this).Handle;
        ToggleControlBox(ControlBox);
    }


    protected internal void ToggleControlBox(bool? enable = null)
    {
        var gwlStyle = User32.GetWindowLongPtr(_hWnd, GWL_STYLE);
        var flag = enable switch
        {
            true  => gwlStyle | WS_SYSMENU,
            false => gwlStyle & ~WS_SYSMENU,
            _     => gwlStyle ^ WS_SYSMENU
        };
        User32.SetWindowLongPtr(_hWnd, GWL_STYLE, flag);
    }

    // ReSharper disable InconsistentNaming
    private const int GWL_STYLE = -16;

    private const int WS_SYSMENU = 0x80000;
    // ReSharper restore InconsistentNaming
}