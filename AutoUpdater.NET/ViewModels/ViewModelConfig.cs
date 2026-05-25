// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Modifiers;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using AutoUpdaterDotNET.Converters;
using WindowService.ViewModels;

namespace AutoUpdaterDotNET.ViewModels;

/// <summary>
///     Default Constructor
/// </summary>
public partial class ViewModelConfig(BaseServiceDependencies dependencies, IViewModelDownloadUpdate vmDownloadUpdate) : ViewModelRestricted(dependencies), IViewModelConfig
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private Config? _configOrig; // Shadow copy
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public IConfig Config
    {
        get => vmDownloadUpdate.Download.Config;
        private set
        {
            vmDownloadUpdate.Download.Config = value;
            _configOrig = JsonSerializer.Deserialize<Config>(JsonSerializer.Serialize(value, _jso), _jso);
            Register?.Invoke((Config)value);
        }
    }

    internal Action<Config>? Register { get; set; }

    public static IEnumerable<RemindLaterFormat> RemindLaterFormatEnumValues => Enum.GetValues<RemindLaterFormat>().Skip(1);
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    private static readonly JsonSerializerOptions _jso = new()
    {
        WriteIndented       = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        Converters          =
        {
            new ConfigInitializationConverter()
        },
        TypeInfoResolver    = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { Modifier.AlphabetizeProperties }
        }
    };
}