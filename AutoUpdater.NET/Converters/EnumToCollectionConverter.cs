// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     EnumToCollectionConverter.cs
// Author:   Latency McLaughlin
// Date:     01/14/2026
// ****************************************************************************

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Models;

namespace AutoUpdaterDotNET.Converters;

[ValueConversion(typeof(Enum), typeof(IEnumerable<ValueDescription>))]
public class EnumToCollectionConverter : MarkupExtension, IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value == null ? null : EnumHelper.GetAllValuesAndDescriptions(value.GetType());

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;

    public override object? ProvideValue(IServiceProvider serviceProvider) => this;
}