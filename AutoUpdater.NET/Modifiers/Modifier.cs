// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Modifier.cs
// Author:   Latency McLaughlin
// Date:     06/19/2025
// ****************************************************************************

using System.Text.Json.Serialization.Metadata;

namespace AutoUpdaterDotNET.Modifiers;

public static class Modifier
{
    public static void AlphabetizeProperties(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
            return;

        var properties = typeInfo.Properties.OrderBy(p => p.Name, StringComparer.Ordinal).ToList();
        typeInfo.Properties.Clear();
        for (var i = 0; i < properties.Count; i++)
        {
            properties[i].Order = i;
            typeInfo.Properties.Add(properties[i]);
        }
    }
}