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
using AutoUpdaterDotNET.TypeResolvers;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;
using WindowService.ViewModels;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig : ViewModelRestricted, IViewModelConfig
{
    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    // Shadow copy
    private readonly Config _config, _configOrig;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public IConfig Config => _config;


    [ObservableProperty]
    public partial IEnumerable<RemindLaterFormat> RemindLaterFormatEnumValues { get; set; } = Enum.GetValues<RemindLaterFormat>().Skip(1);

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public ViewModelConfig(BaseServiceDependencies dependencies) : base(dependencies)
    {
        _config = new()
        {
            EqualsPredicate = new Lazy<Func<bool>>(() => _configOrig?.Equals(_config) ?? true)
        };
        _configOrig = new(_config);
    }


    private static readonly JsonSerializerOptions _jso = new()
    {
        WriteIndented       = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        TypeInfoResolver    = new DependancyPropertyTypeResolver<Config>
        {
            Modifiers = { Modifier.AlphabetizeProperties }
        }
    };
}