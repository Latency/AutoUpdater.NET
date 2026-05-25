// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ConfigInitializationConverter.cs
// Author:   Latency McLaughlin
// Date:     05/25/2026
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Converters;

internal class ConfigInitializationConverter : JsonConverter<Config>
{
    public override Config? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Create options without this specific converter to avoid recursion
        var modifiedOptions   = new JsonSerializerOptions(options);
        var converterToRemove = modifiedOptions.Converters.FirstOrDefault(c => c is ConfigInitializationConverter);
        if (converterToRemove != null) modifiedOptions.Converters.Remove(converterToRemove);

        var config = JsonSerializer.Deserialize<Config>(ref reader, modifiedOptions);

        if (config?.FtpProfile?.Encoding is null || config.FtpProfile?.Encoding.CodePage == 0)
            config?.FtpProfile?.Encoding = new Encoding2(Encodings.Default);

        return config;
    }

    public override void Write(Utf8JsonWriter writer, Config value, JsonSerializerOptions options)
    {
        // For writing, simply use the built-in serializer behavior for the derived types
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}