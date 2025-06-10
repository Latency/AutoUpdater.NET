// ****************************************************************************
// Project:  Patch1
// File:     ViewModelMain.cs
// Author:   Latency McLaughlin
// Date:     05/03/2024
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Commands;
using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Views;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using NullReferenceException = System.NullReferenceException;

namespace AutoUpdaterDotNET.ViewModels;

public sealed class ViewModelMain : DependencyObject, IViewModelMain
{
    public event NotifyCollectionChangedEventHandler?     CollectionChanged;
    public event AutoUpdater.ApplicationExitEventHandler? ApplicationExit;
    public event AutoUpdater.CheckForUpdateEventHandler?  CheckForUpdates;
    public event AutoUpdater.ParseUpdateInfoHandler?      ParseUpdateInfo;


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


    public bool                               IsManditory              { get; set; }
    public bool                               ShowSkipButton           { get; set; }
    public bool                               ShowRemindLaterButton    { get; set; }
    public bool                               RunUpdateAsAdmin         { get; set; }
    public bool                               OpenDownloadPage         { get; set; }
    public bool                               LetUserSelectRemindLater { get; set; }
    public RemindLaterFormat                  RemindLaterTimeSpan      { get; set; }
    public ushort                             RemindLaterAt            { get; set; }
    public string                             AppTitle                 { get; set; }
    public bool                               ReportErrors             { get; set; }
    public bool                               TimerEnabled             { get; set; }
    public RemindLaterFormat                  TimerDurationTimeSpan    { get; set; }
    public ushort                             Interval                 { get; set; }
    public ObservableCollection<TreeViewItem> TimerNodeList            { get; set; } = [];
    public ObservableCollection<TreeViewItem> ApplicationExitNodeList  { get; set; } = [];
    public ObservableCollection<TreeViewItem> CheckForUpdatesNodeList  { get; set; } = [];
    public ObservableCollection<TreeViewItem> ParseUpdateInfoNodeList  { get; set; } = [];
    public bool                               ProxyEnabled             { get; set; }

    [Url]
    public string                             ProxyUri                 { get; set; }

    public string      ProxyUsername                 { get; set; }
    public string      ProxyPassword                 { get; set; }
    public Mode        UpdateMode                    { get; set; }
    public bool        BasicAuth                     { get; set; }
    public bool        BasicAuthDownload             { get; set; }
    public bool        BasicAuthChangeLog            { get; set; }
    public string      BasicAuthUsername             { get; set; }
    public string      BasicAuthPassword             { get; set; }
    public bool        FTPProtocol                   { get; set; }
    public bool        PersistSettings               { get; set; }
    public bool        UseZipFile                    { get; set; }
    public bool        ChangeUpdateZipExtractionPath { get; set; }
    public string      InstallationPath              { get; set; }
    public bool        CheckSynchronously            { get; set; }
    public bool        InstalledVersionOverride      { get; set; }
    public ushort      MajorVersion                  { get; set; }
    public ushort      MinorVersion                  { get; set; }
    public ushort      SubPatchVersion               { get; set; }
    public ushort      BuildVersion                  { get; set; }
    public bool        ClearAppDirectory             { get; set; }
    public bool        ExecutablePathOverride        { get; set; }
    public string      ExecutablePath                { get; set; }
    public bool        BindOwnerWindow               { get; set; } = true;
    public bool        TopMostEnabled                { get; set; } = true;
    public bool        IconOverride                  { get; set; }
    public ImageSource TmpIcon                       { get; private set; }

    public string ImageUri
    {
        get;
        set
        {
            field = value;
            var uri    = new Uri(field, (field.StartsWith("pack:") ? UriKind.Absolute : UriKind.Relative));
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource   = uri;
            bitmap.EndInit();

            TmpIcon = bitmap;

            CollectionChanged?.Invoke(bitmap, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    #endregion Properties


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

        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        timer.Tick += Timer_OnTick;
        timer.Start();

        InvocationListGenerator(timer, nameof(timer.Tick),      TimerNodeList);
        InvocationListGenerator(this,  nameof(ApplicationExit), ApplicationExitNodeList);
        InvocationListGenerator(this,  nameof(CheckForUpdates), CheckForUpdatesNodeList);
        InvocationListGenerator(this,  nameof(ParseUpdateInfo), ParseUpdateInfoNodeList);

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
            {
                root.Items.Add(new TreeViewItem
                {
                    Header = $"{z++}.  {y.GetValue(obj)!.GetType().Name} {signature.Method.Name}"
                });
            }
        }
    }


    private void Timer_OnTick(object? sender, EventArgs e)
    { }


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
        var win = sender as Window_Main ?? throw new NullReferenceException();

    }


    void IViewModelMain.ButtonLoadConfig_Click(object? sender)
    {
        var win = sender as Window_Main ?? throw new NullReferenceException();

    }


    void IViewModelMain.Image_Click(object? sender)
    {
        var img = sender as Image;

        var fd = new OpenFileDialog
        {
            FileName = "Select an image",
            Title = "Icon Image Selection",
            DefaultExt = "All Pictures",
            RestoreDirectory = true,
            Filter = ImageFilter()
        };
        if (fd.ShowDialog() != true)
            return;

        ImageUri = Path.GetRelativePath(Environment.CurrentDirectory, fd.FileName);

        return;

        static string ImageFilter() => "All Files (*.*)|*.*"                                                                                                      +
                                       "|All Pictures (*.emf;*.wmf;*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.bmp;*.dib;*.rle;*.gif;*.emz;*.wmz;*.tif;*.tiff;*.svg;*.ico)" +
                                       "|*.emf;*.wmf;*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.bmp;*.dib;*.rle;*.gif;*.emz;*.wmz;*.tif;*.tiff;*.svg;*.ico"                +
                                       "|Windows Enhanced Metafile (*.emf)|*.emf"                                                                                 +
                                       "|Windows Metafile (*.wmf)|*.wmf"                                                                                          +
                                       "|JPEG File Interchange Format (*.jpg;*.jpeg;*.jfif;*.jpe)|*.jpg;*.jpeg;*.jfif;*.jpe"                                      +
                                       "|Portable Network Graphics (*.png)|*.png"                                                                                 +
                                       "|Bitmap Image File (*.bmp;*.dib;*.rle)|*.bmp;*.dib;*.rle"                                                                 +
                                       "|Compressed Windows Enhanced Metafile (*.emz)|*.emz"                                                                      +
                                       "|Compressed Windows MetaFile (*.wmz)|*.wmz"                                                                               +
                                       "|Tag Image File Format (*.tif;*.tiff)|*.tif;*.tiff"                                                                       +
                                       "|Scalable Vector Graphics (*.svg)|*.svg"                                                                                  +
                                       "|Icon (*.ico)|*.ico";
    }
}