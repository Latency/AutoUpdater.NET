// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     EncodingConverter.cs
// Author:   Latency McLaughlin
// Date:     05/24/2026
// ****************************************************************************

using System.Globalization;
using System.Windows.Data;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;

namespace AutoUpdaterDotNET.Converters;

public class EncodingConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Encoding2 encoding)
            return null;

        var enm = encoding.HeaderName switch
        {
            "utf-16be"   => Encodings.BigEndianUnicode,
            "iso-8859-1" => Encodings.Latin1,
            "utf-8"      => Encodings.UTF8,
            "utf-16"     => Encodings.Unicode,
            "utf-32"     => Encodings.UTF32,
            "us-ascii"   => Encodings.ASCII,
            _            => Encodings.Default
        };
        return new Encoding2(enm);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => new Encoding2((Encodings)value!);
}