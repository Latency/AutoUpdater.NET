// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     BooleanToWindowConverter.cs
// Author:   Latency McLaughlin
// Date:     01/06/2026
// ****************************************************************************

using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Converters;

public class BooleanToWindowConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => !(bool)value! ? (Window?)parameter : null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}