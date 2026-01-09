// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AssemblyLoader;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.TypeResolvers;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Modifiers;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig : ViewModelMainConfig
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
    public event Action<UpdateInfoEventArgs>?      CheckForUpdates;
    public event Action<ParseUpdateInfoEventArgs>? ParseUpdateInfo;
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


    public void OnLoaded()
    {
        UpdateTimer.Interval = GetRemindLaterInterval(TimerInterval);
        UpdateTimer.Tick     += (_, _) => { };

        UpdateIcon += OnUpdateIcon;

        InvocationListGenerator(UpdateTimer, nameof(UpdateTimer.Tick), TimerNodeList);
        InvocationListGenerator(this,        nameof(ApplicationExit),  ApplicationExitNodeList);
        InvocationListGenerator(this,        nameof(CheckForUpdates),  CheckForUpdatesNodeList);
        InvocationListGenerator(this,        nameof(ParseUpdateInfo),  ParseUpdateInfoNodeList);

        LoadConfig();

        SetVersion();
        return;

        // -------------------------------------------
        TimeSpan GetRemindLaterInterval(ushort interval)  => TimerDurationTimeSpan switch
        {
            RemindLaterFormat.Seconds => TimeSpan.FromSeconds(interval),
            RemindLaterFormat.Minutes => TimeSpan.FromMinutes(interval),
            RemindLaterFormat.Hours   => TimeSpan.FromHours  (interval),
            RemindLaterFormat.Days    => TimeSpan.FromDays   (interval),
            _                         => throw new ArgumentOutOfRangeException(nameof(interval))
        };

        static void InvocationListGenerator<T>(T obj, string eventHandlerName, ObservableCollection<TreeViewItem> nodeList)
            where T : class
        {
            var y = typeof(T).GetField(eventHandlerName, BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? throw new NullReferenceException("Field not found in type.");
            var root = new TreeViewItem
            {
                Header = $"On{eventHandlerName} (Delegates)"
            };
            nodeList.Add(root);

            // Delegate was not registered within the object.
            if (y.GetValue(obj) is not Delegate x)
            {
                root.Items.Add(new TreeViewItem
                {
                    Header = "(None)"
                });
                return;
            }

            var z = 1;
            foreach (var signature in x.GetInvocationList())
                root.Items.Add(new TreeViewItem
                {
                    Header = $"{z++}.  {y.GetValue(obj)!.GetType().Name} {signature.Method.Name}"
                });
        }
    }


    private void OnUpdateIcon(BitmapImage? img) => _configOrig.TmpIcon = img;
}