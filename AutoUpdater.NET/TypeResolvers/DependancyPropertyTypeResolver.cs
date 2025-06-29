// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     DependancyPropertyTypeResolver.cs
// Author:   Latency McLaughlin
// Date:     06/19/2025
// ****************************************************************************

using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using AutoUpdaterDotNET.ViewModels;

namespace AutoUpdaterDotNET.TypeResolvers;

public sealed class DependancyPropertyTypeResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = base.GetTypeInfo(type, options);

        foreach (var prop in jsonTypeInfo.Properties.OrderBy(p => p.Name).ToArray())
        {
            if (prop.DeclaringType != typeof(ViewModelMainConfig))
                jsonTypeInfo.Properties.Remove(prop);
        }

        return jsonTypeInfo;
    }
}