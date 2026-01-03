// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     EnumToBooleanConverter.cs
// Author:   Latency McLaughlin
// Date:     01/02/2026
// ****************************************************************************

using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Converters;

public class EnumToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return DependencyProperty.UnsetValue;

        // Compares the bound property value to the ConverterParameter
        return value.ToString() == parameter.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return DependencyProperty.UnsetValue;

        // If the RadioButton is checked (value is true), return its parameter value
        if ((bool)value)
            return Enum.Parse(targetType, parameter.ToString());
        return Binding.DoNothing; // Important: other unchecked buttons should do nothing
    }
}