// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     FindVisualChild.cs
// Author:   Latency McLaughlin
// Date:     02/17/2026
// ****************************************************************************

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace AutoUpdaterDotNET.Extensions;

public static class VisualExtensions
{
    public static T? FindVisualChild<T>(this DependencyObject obj, string? name = null)
        where T : DependencyObject
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        {
            var child = VisualTreeHelper.GetChild(obj, i);
            if (child is T dependencyObject)
            {
                var ctrl = dependencyObject as Control;
                if (!string.IsNullOrEmpty(name))
                {
                    if (ctrl?.Name == name)
                        return dependencyObject;
                }
                else
                    return dependencyObject;
            }

            var childOfChild = child.FindVisualChild<T>(name);
            if (childOfChild != null)
                return childOfChild;
        }

        return null;
    }


    public static PropertyItem? FindProperty(this PropertyGrid propertygrid, string name) => propertygrid.Properties?.Cast<PropertyItem>().First(x => x.PropertyName == name);
}