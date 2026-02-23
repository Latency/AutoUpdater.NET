// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     PasswordBoxContentTemplate.cs
// Author:   Latency McLaughlin
// Date:     02/23/2026
// ****************************************************************************

using System.Windows;
using System.Windows.Controls;

namespace AutoUpdaterDotNET.DataTemplateSelectors;

internal class PasswordBoxContentTemplate : DataTemplateSelector
{
    public required DataTemplate SecurePasswordTemplate    { get; init; }
    public required DataTemplate UnsecuredPasswordTemplate { get; init; }


    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is bool isSecurePassword)
            return isSecurePassword ? SecurePasswordTemplate : UnsecuredPasswordTemplate;

        return null; // Fallback
    }
}