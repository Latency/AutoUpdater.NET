// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     InverseBooleanConverter.cs
// Author:   Latency McLaughlin
// Date:     01/06/2026
// ****************************************************************************

using System.Globalization;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Converters;

public class InverseBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => ApplySetting(value);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => ApplySetting(value);

    private static bool ApplySetting(object? value) => !(bool)value!;
}