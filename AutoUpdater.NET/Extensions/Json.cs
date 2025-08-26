// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Json.cs
// Author:   Latency McLaughlin
// Date:     08/10/2025
// ****************************************************************************

using System.Text.Json;

namespace AutoUpdaterDotNET.Extensions;

public static class JsonExtensions
{
    public static JsonSerializerOptions CopyAndRemoveConverter(this JsonSerializerOptions options, Type converterType)
    {
        var copy = new JsonSerializerOptions(options);
        for (var i = copy.Converters.Count - 1; i >= 0; i--)
            if (copy.Converters[i].GetType() == converterType)
                copy.Converters.RemoveAt(i);
        return copy;
    }
}