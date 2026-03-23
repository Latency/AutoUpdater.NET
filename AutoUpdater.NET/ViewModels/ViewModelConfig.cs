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
using WindowService.ViewModels;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig : ViewModelRestricted, IViewModelConfig
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private Config _config     = new();
    private Config _configOrig = new(); // Shadow copy
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public   IConfig         Config   => _config;
    internal Action<Config>? Register { get; set; }

    public static IEnumerable<RemindLaterFormat> RemindLaterFormatEnumValues => Enum.GetValues<RemindLaterFormat>().Skip(1);

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public ViewModelConfig(BaseServiceDependencies dependencies) : base(dependencies)
    {
    }


    private static readonly JsonSerializerOptions _jso = new()
    {
        WriteIndented       = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        TypeInfoResolver    = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { Modifier.AlphabetizeProperties }
        }
    };
}