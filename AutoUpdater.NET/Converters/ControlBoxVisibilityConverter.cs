// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ControlBoxVisibilityConverter.cs
// Author:   Latency McLaughlin
// Date:     01/02/2026
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using System.Globalization;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Converters;

public class ControlBoxVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => (Mode)value! is not (Mode.Forced or Mode.ForcedDownload);

    public object ConvertBack(object? value, Type targetTypes, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}