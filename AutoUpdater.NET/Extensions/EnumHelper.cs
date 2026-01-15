// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     EnumHelper.cs
// Author:   Latency McLaughlin
// Date:     01/14/2026
// ****************************************************************************

using System.ComponentModel;
using System.Globalization;
using AutoUpdaterDotNET.Models;

namespace AutoUpdaterDotNET.Extensions;

public static class EnumHelper
{
    public static string? Description(this Enum value)
    {
        var attributes = value.GetType().GetField(value.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes is not null && attributes.Length != 0)
            return (attributes.First() as DescriptionAttribute)?.Description;

        // If no description is found, the least we can do is replace underscores with spaces
        // You can add your own custom default formatting logic here
        var ti = CultureInfo.CurrentCulture.TextInfo;
        return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
    }

    public static IEnumerable<ValueDescription> GetAllValuesAndDescriptions(Type t)
    {
        if (!t.IsEnum)
            throw new ArgumentException($"{nameof(t)} must be an enum type");

        return Enum.GetValues(t)
                   .Cast<Enum>()
                   .Select(e => new ValueDescription
                   {
                       Value       = e,
                       Description = e.Description()
                   })
                   .ToList();
    }
}