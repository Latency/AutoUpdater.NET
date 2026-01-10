// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     DependancyObjectHelper.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace AutoUpdaterDotNET.Extensions;

public static class DependencyObjectHelper
{
    public static IEnumerable<BindingBase> GetBindings(this DependencyObject? obj)
    {
        if (obj == null)
            yield break;

        // Use reflection to find all static DependencyProperty fields for the object's type
        var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        foreach (var field in fields)
        {
            if (field.FieldType != typeof(DependencyProperty))
                continue;

            var dp      = (DependencyProperty?)field.GetValue(null);
            var binding = BindingOperations.GetBindingBase(obj, dp!);

            if (binding != null)
                yield return binding;
        }
    }


    public static BindingBase? GetBindings2(this DependencyProperty? dp, DependencyObject? obj)
    {
        if (dp == null || obj == null)
            return null;

        return BindingOperations.GetBindingBase(obj, dp);
    }
}