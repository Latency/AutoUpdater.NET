// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Restricted.cs
// Author:   Latency McLaughlin
// Date:     01/07/2026
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Converters;
using AutoUpdaterDotNET.Interops;
using AutoUpdaterDotNET.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;
namespace AutoUpdaterDotNET.Controls;

/// <summary>
///     Interaction logic for Window_Restricted.xaml
/// </summary>
public abstract class Window_Restricted : Window
{
    public static readonly DependencyProperty ControlBoxProperty = DependencyProperty.Register(nameof(ControlBox),                                        // Name of the property
                                                                                               typeof(bool?),                                             // Type of the property
                                                                                               typeof(Window_Restricted),                                 // Owner class type
                                                                                               new FrameworkPropertyMetadata(true, OnControlBoxChanged)); // Property metadata (default value, property changed callback, etc.)

    public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(nameof(Owner),                                  // Name of the property
                                                                                          typeof(Window),                                 // Type of the property
                                                                                          typeof(Window_Restricted),                      // Owner class type
                                                                                          new FrameworkPropertyMetadata(OnOwnerChanged)); // Property metadata (default value, property changed callback, etc.)

    #region Constructor
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    /// <summary>
    ///     Default Constructor
    /// </summary>
    protected Window_Restricted()
    {
        ControlBox            = true;
        ResizeMode            = ResizeMode.NoResize;
        ShowActivated         = true;
        ShowInTaskbar         = true;
        Topmost               = true;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        WindowStyle           = WindowStyle.SingleBorderWindow;

        Loaded  += OnLoaded;
        Closing += OnClosing;
    }

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Constructor


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public new Window? Owner
    {
        get => (Window?)GetValue(OwnerProperty);
        set
        {
            base.Owner = value!;
            SetValue(OwnerProperty, value!);
        }
    }

    public bool? ControlBox
    {
        get => (bool?)GetValue(ControlBoxProperty);
        set => SetValue(ControlBoxProperty, value!);
    }

    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    // ReSharper disable InconsistentNaming
    private const int GWL_STYLE = -16;

    private const int WS_SYSMENU = 0x80000;
    // ReSharper restore InconsistentNaming

    // Handle to current window.
    private static nint _hWnd;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _hWnd = new WindowInteropHelper(this).Handle;

        if (DataContext is not ViewModelRestricted vm)
            throw new NullReferenceException();

        SetBinding(ControlBoxProperty, new Binding
        {
            Source    = vm.Config,
            Path      = new PropertyPath(nameof(vm.Config.UpdateMode)),
            Converter = new ControlBoxVisibilityConverter()
        });

        SetBinding(TopmostProperty, new Binding
        {
            Source    = vm.Config,
            Path      = new PropertyPath(nameof(vm.Config.TopMostDisabled)),
            Converter = new InverseBooleanConverter()
        });

        SetBinding(OwnerProperty, new Binding
        {
            Source    = vm.Config,
            Path      = new PropertyPath(nameof(vm.Config.DoNotBindOwnerWindow)),
            Converter = new BooleanToWindowConverter(),
            ConverterParameter = this
        });
    }


    private static void OnOwnerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        #if DEBUG
        var ctrl         = (Window) d;
        var ownerEnabled = e.NewValue is not null;
        var newValue = e.NewValue as Window;
        ctrl.Tag   = (e.OldValue as Window)!;
        ctrl.Title = ownerEnabled ? $"{ctrl.Title}{(ownerEnabled ? $" (Owner: {newValue?.GetType()})" : string.Empty)}" : ctrl.Title![..ctrl.Title.IndexOf('(')].TrimEnd();
        ctrl.Owner = newValue!;
        #endif
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


    private void OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}