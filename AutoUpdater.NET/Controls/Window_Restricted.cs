// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_Restricted.cs
// Author:   Latency McLaughlin
// Date:     01/07/2026
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Converters;
using AutoUpdaterDotNET.Interops;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Interfaces;
using Xceed.Wpf.Toolkit.Core.Converters;

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

    public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(nameof(Owner2),                                 // Name of the property
                                                                                          typeof(IFrameworkInputElement),                 // Type of the property
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

    public IViewModelMainConfig? Config { get; set; }

    public IFrameworkInputElement? Owner2
    {
        get => (IFrameworkInputElement?)GetValue(OwnerProperty);
        set => SetValue(OwnerProperty, value!);
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
    private const int GWL_STYLE = -16;
    private const int WS_SYSMENU = 0x80000;

    // Handle to current window.
    private static nint _hWnd;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        _hWnd = new WindowInteropHelper(this).Handle;

        SetBinding(ControlBoxProperty, new Binding
        {
            Source    = Config!,
            Path      = new PropertyPath(nameof(Config.UpdateMode)),
            Converter = new ControlBoxVisibilityConverter()
        });

        SetBinding(TopmostProperty, new Binding
        {
            Source    = Config!,
            Path      = new PropertyPath(nameof(Config.TopMostDisabled)),
            Converter = new InverseBoolConverter()
        });

        SetBinding(OwnerProperty, new Binding
        {
            Source             = Config!,
            Path               = new PropertyPath(nameof(Config.DoNotBindOwnerWindow)),
            Converter          = new BooleanToWindowConverter(),
            ConverterParameter = Owner!
        });


        var b = GetBindings();
    }


    public IEnumerable<BindingBase> GetBindings()
    {
        List<DependencyProperty> a    = [ControlBoxProperty, OwnerProperty, TopmostProperty];
        List<BindingBase?>  test = [];

        foreach (var b in a)
        {
            var c = b.GetBindings2(this);
            test.Add(c);
        }

        //var                      c    = a.SelectMany(x => x.GetBindings2(this));
        return test;
    }


    private static void OnOwnerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctrl     = (Window)d;
        var newValue = e.NewValue as Window;
        ctrl.Owner   = newValue!;
#if DEBUG
        var ownerEnabled = e.NewValue is not null;
        ctrl.Title = ownerEnabled ? $"{ctrl.Title}{(ownerEnabled ? $" (Owner: {newValue?.GetType()})" : string.Empty)}" : ctrl.Title![..ctrl.Title.IndexOf('(')].TrimEnd();
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