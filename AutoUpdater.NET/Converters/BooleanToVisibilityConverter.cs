// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     BooleanToVisibilityConverter.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Converters;

// ReSharper disable once InconsistentNaming
public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => (bool)value! ? Visibility.Visible : Visibility.Hidden;

    public object ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}