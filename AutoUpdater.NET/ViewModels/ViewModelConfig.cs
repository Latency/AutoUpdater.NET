// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AssemblyLoader;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Modifiers;
using AutoUpdaterDotNET.TypeResolvers;
using System.Text.Json;
using System.Windows.Threading;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig : ViewModelMainConfig, IViewModelConfig
{
    // Shadow copy
    private readonly ViewModelMainConfig _configOrig;


    /// <summary>
    ///     Default Constructor
    /// </summary>
    /// <param name="serviceProvider"></param>
    public ViewModelConfig(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _configOrig = new ViewModelMainConfig(this, serviceProvider);
    }


    private static readonly JsonSerializerOptions _jso = new()
    {
        WriteIndented       = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        TypeInfoResolver    = new DependancyPropertyTypeResolver<ViewModelMainConfig>
        {
            Modifiers = { Modifier.AlphabetizeProperties }
        }
    };


    public override bool Equals() => _configOrig.Equals(this);


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    public DispatcherTimer UpdateTimer { get; } = new();
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=+
    public event Action?                           ApplicationExit;
    //public event Action<UpdateInfoEventArgs>?      CheckForUpdates;
    //public event Action<ParseUpdateInfoEventArgs>? ParseUpdateInfo;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    private void SetVersion()
    {
        if (!InstalledVersionOverride)
        {
            var defaultVersion = GetType().Assembly.Version()!;

            MajorVersion    = (ushort)defaultVersion.Major;
            MinorVersion    = (ushort)defaultVersion.Minor;
            BuildVersion    = (ushort)defaultVersion.Build;
            RevisionVersion = (ushort)defaultVersion.Revision;
        }

        base.Update();
    }
}