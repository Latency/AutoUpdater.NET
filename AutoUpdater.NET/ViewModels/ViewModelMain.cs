// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelMain.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AssemblyLoader;
using AutoUpdaterDotNET.Commands;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Views;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Exception = System.Exception;

namespace AutoUpdaterDotNET.ViewModels;

public sealed class ViewModelMain : ViewModelMainConfig, IViewModelMain
{
    private ViewModelMainConfig? _configOrig;
    private static readonly JsonSerializerOptions _jso = new()
    {
        WriteIndented = true,
        //TypeInfoResolver = new DependancyPropertyTypeResolver
        //{
        //    Modifiers = { JsonExtensions.AlphabetizeProperties }
        //}
    };


    /// <summary>
    ///     Constructor
    /// </summary>
    public ViewModelMain()
    {
        CommandCancel      = new RelayCommand(((IViewModelMain)this).ButtonCancel_Click,     AllowCancel);
        CommandUpdate      = new RelayCommand(((IViewModelMain)this).ButtonUpdate_Click,     AllowUpdate);
        CommandSaveConfig  = new RelayCommand(((IViewModelMain)this).ButtonSaveConfig_Click, AllowSave);
        CommandLoadConfig  = new RelayCommand(((IViewModelMain)this).ButtonLoadConfig_Click, AllowLoad);
        CommandImageChange = new RelayCommand(((IViewModelMain)this).Image_Click,            AllowImageChange);
    }

    public bool Equals() => _configOrig!.Equals(this);


    #region Events
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=+

    public event IViewModelMain.ApplicationExitEventHandler? ApplicationExit;
    public event IViewModelMain.CheckForUpdateEventHandler?  CheckForUpdates;
    public event IViewModelMain.ParseUpdateInfoHandler?      ParseUpdateInfo;
    public event PropertyChangedEventHandler?                PropertyChanged;

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Events


    void IViewModelMain.OnLoaded(object? sender)
    {
        Func<ushort, TimeSpan> ts = TimerDurationTimeSpan switch
        {
            RemindLaterFormat.Seconds => s => TimeSpan.FromSeconds(s),
            RemindLaterFormat.Minutes => m => TimeSpan.FromMinutes(m),
            RemindLaterFormat.Hours   => h => TimeSpan.FromHours(h),
            RemindLaterFormat.Days    => d => TimeSpan.FromDays(d),
            _                         => throw new ArgumentOutOfRangeException()
        };
        UpdateTimer.Interval =  ts.Invoke(Interval);
        UpdateTimer.Tick     += (_, _) => { };

        InvocationListGenerator(UpdateTimer, nameof(UpdateTimer.Tick), TimerNodeList);
        InvocationListGenerator(this,        nameof(ApplicationExit),  ApplicationExitNodeList);
        InvocationListGenerator(this,        nameof(CheckForUpdates),  CheckForUpdatesNodeList);
        InvocationListGenerator(this,        nameof(ParseUpdateInfo),  ParseUpdateInfoNodeList);

        _configOrig = new ViewModelMainConfig(this);

        if (sender is Window_Main { cbInstalledVersionOverride: not null } vm)
            vm.cbInstalledVersionOverride.IsChecked = false;

        var defaultVersion = GetType().Assembly.Version()!;
        MajorVersion    = (ushort) defaultVersion.Major;
        MinorVersion    = (ushort) defaultVersion.Minor;
        BuildVersion    = (ushort) defaultVersion.Build;
        RevisionVersion = (ushort) defaultVersion.Revision;

        // Override version from configuration loading here...

        return;

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


    void IViewModelMain.ButtonCancel_Click(object? sender)
    {
        var win = sender as Window_Main ?? throw new NullReferenceException();
        win.Hide();
    }


    void IViewModelMain.ButtonUpdate_Click(object? sender)
    {
        var win = sender as Window_Main ?? throw new NullReferenceException();
        win.Hide();
    }


    void IViewModelMain.ButtonSaveConfig_Click(object? sender)
    {
        var win       = sender as Window_Main ?? throw new NullReferenceException();
        var directory = $@"{Directory.GetCurrentDirectory()}\Properties";

        var s = new SaveFileDialog
        {
            DefaultDirectory = directory,
            FileName         = "AutoUpdate.json",
            Filter           = "All Files (*.*)|*.*|Json Files (*.json)|*.json"
        };
        var result = s.ShowDialog(win);
        if (result is null or false)
            return;

        try
        {
            var json = JsonSerializer.Serialize<ViewModelMainConfig>(this, _jso);
            File.WriteAllTextAsync(s.FileName, json);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }
    }


    void IViewModelMain.ButtonLoadConfig_Click(object? sender)
    {
        var win = sender as Window_Main ?? throw new NullReferenceException();
    }


    void IViewModelMain.Image_Click(object? sender)
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


    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private static bool AllowCancel(object?      _) => true;
    private static bool AllowUpdate(object?      _) => true;
    private static bool AllowSave(object?        _) => true;
    private static bool AllowLoad(object?        _) => true;
    private static bool AllowImageChange(object? _) => true;

    public ICommand CommandCancel      { get; set; }
    public ICommand CommandUpdate      { get; set; }
    public ICommand CommandSaveConfig  { get; set; }
    public ICommand CommandLoadConfig  { get; set; }
    public ICommand CommandImageChange { get; set; }

    public DispatcherTimer UpdateTimer { get; } = new();
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    private void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}