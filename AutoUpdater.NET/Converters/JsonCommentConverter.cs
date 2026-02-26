// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     JsonCommentConverter.cs
// Author:   Latency McLaughlin
// Date:     08/10/2025
// ****************************************************************************

using System.Text.Json;

namespace AutoUpdaterDotNET.Converters;

public class JsonCommentConverter(string comment) : JsonCommentConverter<object>(comment);


public class JsonCommentConverter<TBase>(string comment) : DefaultConverterFactory<TBase>
{
    protected override void Write<T>(Utf8JsonWriter writer, T value, JsonSerializerOptions modifiedOptions)
    {
        var lines = comment.Split('\n');

        if (lines.Length > 1)
        {
            JsonSerializer.Serialize(writer, value, modifiedOptions);
            foreach (var line in lines)
                writer.WriteCommentValue(line);
        }
        else
        {
            // TODO: in .NET 9 investigate https://learn.microsoft.com/en-us/dotnet/api/microsoft.toolkit.highperformance.buffers.arraypoolbufferwriter-1 to avoid string allocations.
            var str = JsonSerializer.Serialize(value, modifiedOptions);
            writer.WriteRawValue($"{str} /*{comment}*/", skipInputValidation: true);
        }
    }

    protected override JsonSerializerOptions ModifyOptions(JsonSerializerOptions options)
    {
        var modifiedOptions = base.ModifyOptions(options);
        modifiedOptions.WriteIndented = false;
        return modifiedOptions;
    }
}