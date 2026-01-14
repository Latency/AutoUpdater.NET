// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     BaseConverter.cs
// Author:   Latency McLaughlin
// Date:     01/13/2026
// ****************************************************************************

using System.Globalization;
using AutoUpdaterDotNET.Interfaces;
using System.Windows;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Dependency_Properties;

public abstract class BaseConverter : DependencyObject, IValueConverter
{
    /// <summary>
    /// Default Constructor
    /// </summary>
    /// <param name="parent"></param>
    protected BaseConverter(Window parent)
    {
        Parent = parent;
    }


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public abstract void SetBindings(IViewModelConfig vm);
    protected void SetBindings(DependencyProperty property, IViewModelConfig vm, string name, object? parameter = null)
    {
        Parent.SetBinding(property, new Binding
        {
            Source              = vm,
            Path                = new PropertyPath(name),
            Converter           = this,
            ConverterParameter =  parameter!,
            Mode                = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });
    }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods


    #region Properties
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected Window Parent { get; }
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    protected abstract object? Converter(object?     value, Type targetType, object? parameter, CultureInfo culture);
    protected virtual  object? BackConverter(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
    object? IValueConverter.   Convert(object?       value, Type targetType, object? parameter, CultureInfo culture) => Converter(value, targetType, parameter, culture);
    object? IValueConverter.   ConvertBack(object?   value, Type targetType, object? parameter, CultureInfo culture) => BackConverter(value, targetType, parameter, culture);
    // =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}