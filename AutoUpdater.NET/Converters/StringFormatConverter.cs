// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     StringFormatConverter.cs
// Author:   Latency McLaughlin
// Date:     12/31/2025
// ****************************************************************************

using System.Globalization;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Converters;

// ReSharper disable once InconsistentNaming
public class StringFormatConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => string.Format((parameter as string)!, value);

    public object ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}