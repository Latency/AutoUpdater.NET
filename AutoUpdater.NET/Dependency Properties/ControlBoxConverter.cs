// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ControlBoxConverter.cs
// Author:   Latency McLaughlin
// Date:     01/13/2026
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Interops;
using System.Globalization;
using System.Windows;
using System.Windows.Interop;

namespace AutoUpdaterDotNET.Dependency_Properties;

public class ControlBoxConverter : BaseConverter
{
    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private const int GWL_STYLE  = -16;
    private const int WS_SYSMENU = 0x80000;

    private readonly DependencyProperty _ownerProperty;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="window"></param>
    public ControlBoxConverter(Window window) : base(window)
    {
        _ownerProperty = DependencyProperty.Register(nameof(ControlBox),
                                                     typeof(bool?),
                                                     window.GetType(),
                                                     new FrameworkPropertyMetadata(OnControlBoxChanged)
        );
    }


    #region Properties
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public bool? ControlBox
    {
        get => (bool?)GetValue(_ownerProperty);
        set => SetValue(_ownerProperty, value!);
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected override object? Converter(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var flag = (Mode)value! is not (Mode.Forced or Mode.ForcedDownload);
        ControlBox = flag;
        return flag;
    }


    private void OnControlBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var _hWnd    = new WindowInteropHelper(Parent).Handle;
        var gwlStyle = User32.GetWindowLongPtr(_hWnd, GWL_STYLE);
        User32.SetWindowLongPtr(_hWnd, GWL_STYLE, e.NewValue switch
        {
            true  => gwlStyle | WS_SYSMENU,
            false => gwlStyle & ~WS_SYSMENU,
            _     => gwlStyle ^ WS_SYSMENU
        });
    }


    /// <summary>
    /// SetBindings
    /// </summary>
    public override void SetBindings(IViewModelConfig vm) => SetBindings(_ownerProperty, vm, nameof(vm.UpdateMode));
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}