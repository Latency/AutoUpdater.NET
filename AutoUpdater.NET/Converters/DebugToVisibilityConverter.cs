// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     DebugToVisibilityConverter.cs
// Author:   Latency McLaughlin
// Date:     01/02/2026
// ****************************************************************************

using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Converters;

public class DebugToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        // The following check works reliably at runtime
        Debugger.IsAttached ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}