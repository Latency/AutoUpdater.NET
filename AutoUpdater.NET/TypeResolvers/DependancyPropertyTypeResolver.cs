// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     DependancyPropertyTypeResolver.cs
// Author:   Latency McLaughlin
// Date:     06/19/2025
// ****************************************************************************

using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Windows;
using System.Windows.Threading;

namespace AutoUpdaterDotNET.TypeResolvers;

public sealed class DependancyPropertyTypeResolver<T> : DefaultJsonTypeInfoResolver
    where T : class
{
    private readonly DefaultJsonTypeInfoResolver _defaultResolver = new();

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        var jsonTypeInfo = _defaultResolver.GetTypeInfo(type, options);

        if (!type.IsAssignableFrom(typeof(T)))
            return jsonTypeInfo;

        // Exclude properties from BaseClass
        foreach (var prop in jsonTypeInfo.Properties.ToList())
        {
            Trace.WriteLine(prop.Name);
            if (prop.DeclaringType == typeof(DependencyObject) || prop.PropertyType == typeof(Dispatcher))
            {
                jsonTypeInfo.Properties.Remove(prop);
                continue;
            }

            var provider = prop.AttributeProvider;
            if (provider is null)
                continue;

            var attrs = provider.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
            foreach (JsonIgnoreAttribute attr in attrs)
            {
                switch (attr.Condition)
                {
                    case JsonIgnoreCondition.Never:
                        break;
                    case JsonIgnoreCondition.Always:
                        jsonTypeInfo.Properties.Remove(prop);
                        break;
                    case JsonIgnoreCondition.WhenWritingDefault:
                    case JsonIgnoreCondition.WhenWritingNull:
                    case JsonIgnoreCondition.WhenWriting:
                    case JsonIgnoreCondition.WhenReading:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        return jsonTypeInfo;
    }
}