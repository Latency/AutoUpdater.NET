// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     BytesToStringConverter.cs
// Author:   Latency McLaughlin
// Date:     02/20/2026
// ****************************************************************************

using System.Globalization;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Converters;

public class BytesToStringConverter : IValueConverter
{
    private static readonly string[] Suf = ["B", "KB", "MB", "GB", "TB", "PB", "EB"];

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var byteCount = (long) (value ?? 0);
        if (byteCount == 0)
            return "0" + Suf[0];

        var bytes = Math.Abs(byteCount);
        var place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        var num   = Math.Round(bytes / Math.Pow(1024, place), 1);
        return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {Suf[place]}";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}