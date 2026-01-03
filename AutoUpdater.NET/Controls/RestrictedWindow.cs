// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     RestrictedWindow.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using AutoUpdaterDotNET.Interops;
using System.Windows;
using System.Windows.Interop;

namespace AutoUpdaterDotNET.Controls;

public abstract class RestrictedWindow : Window
{
    public static readonly DependencyProperty ControlBoxProperty = DependencyProperty.Register(nameof(ControlBox),                                        // Name of the property
                                                                                               typeof(bool?),                                             // Type of the property
                                                                                               typeof(RestrictedWindow),                                  // Owner class type
                                                                                               new FrameworkPropertyMetadata(true, OnControlBoxChanged)); // Property metadata (default value, property changed callback, etc.)

    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    // ReSharper disable InconsistentNaming
    private const int GWL_STYLE  = -16;
    private const int WS_SYSMENU = 0x80000;
    // ReSharper restore InconsistentNaming

    // Handle to current window.
    private static nint _hWnd;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public bool? ControlBox
    {
        get => (bool?)GetValue(ControlBoxProperty);
        set => SetValue(ControlBoxProperty, value!);
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Constructor
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    /// <summary>
    ///     Default Constructor
    /// </summary>
    protected RestrictedWindow()
    {
        ControlBox            = true;
        ResizeMode            = ResizeMode.NoResize;
        ShowActivated         = true;
        ShowInTaskbar         = true;
        Topmost               = true;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        WindowStyle           = WindowStyle.SingleBorderWindow;

        Loaded += Window_OnLoaded;
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Constructor


    private void Window_OnLoaded(object sender, RoutedEventArgs e)
    {
        _hWnd = new WindowInteropHelper(this).Handle;
    }


    private static void OnControlBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        //var control  = (RestrictedWindow)d;
        var oldValue = (bool?)e.OldValue;
        var newValue = (bool?)e.NewValue;

        if (oldValue == newValue)
            return;

        var gwlStyle = User32.GetWindowLongPtr(_hWnd, GWL_STYLE);
        var flag = newValue switch
        {
            true  => gwlStyle | WS_SYSMENU,
            false => gwlStyle & ~WS_SYSMENU,
            _     => gwlStyle ^ WS_SYSMENU
        };
        User32.SetWindowLongPtr(_hWnd, GWL_STYLE, flag);
    }
}