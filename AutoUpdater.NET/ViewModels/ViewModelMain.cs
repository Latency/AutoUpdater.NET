// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelMain.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AssemblyLoader;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.TypeResolvers;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AutoUpdaterDotNET.Models;
using CommunityToolkit.Mvvm.Input;
using Exception = System.Exception;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelMain : ViewModelMainConfig, IViewModelMain
{
    private readonly ViewModelMainConfig _configOrig;


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public ViewModelMain()
    {
        _configOrig = new ViewModelMainConfig(this);
    }


    private static readonly JsonSerializerOptions _jso = new()
    {
        WriteIndented       = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        TypeInfoResolver = new DependancyPropertyTypeResolver<ViewModelMainConfig>
        {
            Modifiers = { JsonExtensions.AlphabetizeProperties }
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
    public event Action<string?>?                  UpdateVersion;
    public event Action<ImageSource?>?             UpdateIcon;
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    void IViewModelMain.OnLoaded(object? sender)
    {
        //_win = sender as Window_Main ?? throw new NullReferenceException();

        UpdateTimer.Interval = GetRemindLaterInterval(TimerInterval);
        UpdateTimer.Tick     += (_, _) => { };

        InvocationListGenerator(UpdateTimer, nameof(UpdateTimer.Tick), TimerNodeList);
        InvocationListGenerator(this,        nameof(ApplicationExit),  ApplicationExitNodeList);
        InvocationListGenerator(this,        nameof(CheckForUpdates),  CheckForUpdatesNodeList);
        InvocationListGenerator(this,        nameof(ParseUpdateInfo),  ParseUpdateInfoNodeList);

        LoadConfig();

        var defaultVersion = GetType().Assembly.Version()!;

        MajorVersion    = (ushort)defaultVersion.Major;
        MinorVersion    = (ushort)defaultVersion.Minor;
        BuildVersion    = (ushort)defaultVersion.Build;
        RevisionVersion = (ushort)defaultVersion.Revision;

        _defaultInstalledVersion = new Version(MajorVersion, MinorVersion, BuildVersion, RevisionVersion);
        UpdateVersion?.Invoke($"Loader Version: {_defaultInstalledVersion}");

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


    [RelayCommand]
    private void Cancel()
    {

    }


    [RelayCommand]
    private void Update()
    {
        UpdateIcon?.Invoke(TmpIcon);
        UpdateVersion?.Invoke(InstalledVersion?.Version?.ToString());


        SaveConfig();

        _configOrig?.Copy(this);
        InstalledVersionOverride = false;

        OnUpdateValidation(null);
    }


    [RelayCommand]
    private void SaveConfig()
    {
        var directory = $@"{Directory.GetCurrentDirectory()}\Properties";
        var file      = $@"{directory}\{Environment.GetEnvironmentVariable("ConfigFile")}";

        //var s = new SaveFileDialog
        //{
        //    DefaultDirectory = directory,
        //    FileName         = config,
        //    Filter           = "All Files (*.*)|*.*|Json Files (*.json)|*.json"
        //};
        //var result = s.ShowDialog(win);
        //if (result is null or false)
        //    return;

        //if (Equals())
        //{

        //    if (File.Exists(file) && File.OpenRead(file).Length == 0)
        //        File.Delete(file);
        //    return;
        //}

        try
        {
            var tmp = new Dictionary<string, object>();

            if (IsManditory)
            {
                tmp.TryAdd(nameof(ShowSkipButton),        ShowSkipButton);
                tmp.TryAdd(nameof(ShowRemindLaterButton), ShowRemindLaterButton);

                ShowSkipButton        = false;
                ShowRemindLaterButton = false;
            }

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var json = JsonSerializer.Serialize<ViewModelMainConfig>(this, _jso);
            File.WriteAllTextAsync(file, json);

            if (IsManditory)
            {
                foreach (var item in tmp)
                    GetType().GetProperty(item.Key)?.SetValue(this, item.Value);

                tmp.Clear();
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }
    }


    [RelayCommand]
    private void LoadConfig()
    {
        const string config = "AutoUpdate.json";
        var          file   = $@"{Directory.GetCurrentDirectory()}\Properties\{config}";

        if (!File.Exists(file))
            return;

        try
        {
            var vmmc = JsonSerializer.Deserialize<ViewModelMainConfig>(file, _jso);
            Copy(vmmc);
            _configOrig.Copy(this);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }
    }


    [RelayCommand]
    private void ImageChange()
    {
        var fd = new OpenFileDialog
        {
            FileName         = "Select an image",
            Title            = "Icon Image Selection",
            DefaultExt       = "All Pictures",
            RestoreDirectory = true,
            Filter           = ImageFilter()
        };
        if (fd.ShowDialog() != true)
            return;

        var imageUri = Path.GetRelativePath(Environment.CurrentDirectory, fd.FileName);
        var uri      = new Uri(imageUri, imageUri.StartsWith("pack:") ? UriKind.Absolute : UriKind.Relative);
        var bitmap   = new BitmapImage();
        bitmap.BeginInit();
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.UriSource   = uri;
        bitmap.EndInit();

        TmpIcon = bitmap;

        OnPropertyChanged(nameof(TmpIcon));

        return;

        static string ImageFilter() => "All Files (*.*)|*.*" +
                                       "|All Pictures (*.emf;*.wmf;*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.bmp;*.dib;*.rle;*.gif;*.emz;*.wmz;*.tif;*.tiff;*.svg;*.ico)" +
                                       "|*.emf;*.wmf;*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.bmp;*.dib;*.rle;*.gif;*.emz;*.wmz;*.tif;*.tiff;*.svg;*.ico" +
                                       "|Windows Enhanced Metafile (*.emf)|*.emf" +
                                       "|Windows Metafile (*.wmf)|*.wmf" +
                                       "|JPEG File Interchange Format (*.jpg;*.jpeg;*.jfif;*.jpe)|*.jpg;*.jpeg;*.jfif;*.jpe" +
                                       "|Portable Network Graphics (*.png)|*.png" +
                                       "|Bitmap Image File (*.bmp;*.dib;*.rle)|*.bmp;*.dib;*.rle" +
                                       "|Compressed Windows Enhanced Metafile (*.emz)|*.emz" +
                                       "|Compressed Windows MetaFile (*.wmz)|*.wmz" +
                                       "|Tag Image File Format (*.tif;*.tiff)|*.tif;*.tiff" +
                                       "|Scalable Vector Graphics (*.svg)|*.svg" +
                                       "|Icon (*.ico)|*.ico";
    }
}