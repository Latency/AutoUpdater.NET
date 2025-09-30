// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     JsonComment.cs
// Author:   Latency McLaughlin
// Date:     08/10/2025
// ****************************************************************************

using System.Text.Json.Serialization;
using AutoUpdaterDotNET.Converters;

namespace AutoUpdaterDotNET.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JsonCommentAttribute(string comment) : JsonConverterAttribute
{
    public override JsonConverter CreateConverter(Type typeToConvert) => new JsonCommentConverter(comment);
}