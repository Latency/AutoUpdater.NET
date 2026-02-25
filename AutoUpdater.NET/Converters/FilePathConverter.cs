// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     FilePathConverter.cs
// Author:   Latency McLaughlin
// Date:     02/24/2026
// ****************************************************************************

using System.Globalization;
using System.Windows.Data;
using AutoUpdaterDotNET.Models;

namespace AutoUpdaterDotNET.Converters;

public class FilePathConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => new FilePath
    {
        Path = value as string ?? string.Empty
    };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}