// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     DefaultConverterFactory.cs
// Author:   Latency McLaughlin
// Date:     08/10/2025
// ****************************************************************************

using System.Text.Json;
using System.Text.Json.Serialization;
using AutoUpdaterDotNET.Extensions;

namespace AutoUpdaterDotNET.Converters;

public abstract class DefaultConverterFactory<TBase> : JsonConverterFactory
{
    protected virtual JsonSerializerOptions ModifyOptions(JsonSerializerOptions options) => options.CopyAndRemoveConverter(GetType());

    protected virtual TConcrete? Read<TConcrete>(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions modifiedOptions)
        where TConcrete : TBase => (TConcrete?)JsonSerializer.Deserialize(ref reader, typeToConvert, modifiedOptions);

    protected virtual void Write<TConcrete>(Utf8JsonWriter writer, TConcrete value, JsonSerializerOptions modifiedOptions)
        where TConcrete : TBase => JsonSerializer.Serialize(writer, value, modifiedOptions);

    public sealed override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var modifiedOptions = ModifyOptions(options);
        if (typeToConvert == typeof(TBase))
            return new DefaultConverter<TBase>(modifiedOptions, this);

        return (JsonConverter)Activator.CreateInstance(typeof(DefaultConverter<>).MakeGenericType(typeof(TBase), typeToConvert), modifiedOptions, this)!;
    }

    public override bool CanConvert(Type typeToConvert) => typeof(TBase).IsAssignableFrom(typeToConvert);

    // Adapted from this answer https://stackoverflow.com/a/78512783/3744182
    // To https://stackoverflow.com/questions/78507408/in-system-text-json-is-it-possible-to-minify-only-array-items
    private sealed class DefaultConverter<TConcrete>(JsonSerializerOptions modifiedOptions, DefaultConverterFactory<TBase> factory) : JsonConverter<TConcrete>
        where TConcrete : TBase
    {
        public override void Write(Utf8JsonWriter writer, TConcrete value, JsonSerializerOptions options) => factory.Write(writer, value, modifiedOptions);

        public override TConcrete? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => factory.Read<TConcrete>(ref reader, typeToConvert, modifiedOptions);
    }
}