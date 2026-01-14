// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     TopmostConverter.cs
// Author:   Latency McLaughlin
// Date:     01/13/2026
// ****************************************************************************

using System.Globalization;
using System.Windows;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Dependency_Properties;

public class TopmostConverter : BaseConverter
{
    #region Fields
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private readonly DependencyProperty _topmostProperty;
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="window"></param>a
    public TopmostConverter(Window window) : base(window)
    {
        _topmostProperty = DependencyProperty.Register(nameof(Topmost),
                                                       typeof(bool),
                                                       window.GetType(),
                                                       new FrameworkPropertyMetadata(OnTopmostChanged)
        );
    }


    #region Properties
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public bool Topmost
    {
        get => (bool)GetValue(_topmostProperty);
        set => SetValue(_topmostProperty, value);
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected override object? Converter(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var flag = !(bool)value!;
        Topmost = flag;
        return flag;
    }


    private void OnTopmostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Parent.Topmost = (bool) e.NewValue;
    }


    /// <summary>
    /// SetBindings
    /// </summary>
    public override void SetBindings(IViewModelConfig vm) => SetBindings(_topmostProperty, vm, nameof(vm.TopMostDisabled));
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}