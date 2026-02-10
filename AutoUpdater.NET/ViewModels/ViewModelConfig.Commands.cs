// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.Commands.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Extensions;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Controls;
using AutoUpdaterDotNET.Properties;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig
{
    [RelayCommand]
    private void Loaded()
    {
        _updateTimer.Interval = GetRemindLaterInterval(TimerInterval);
        _updateTimer.Tick += (_, _) => { };

        UpdateIcon += img => _configOrig.TmpIcon = img;

        InvocationListGenerator(_updateTimer, nameof(_updateTimer.Tick), TimerNodeList);
        InvocationListGenerator(this,         nameof(UpdateComplete),   UpdateCompleteNodeList);
        InvocationListGenerator(this,         nameof(CheckForUpdates),  CheckForUpdatesNodeList);

        LoadConfig();

        SetVersion();
        return;

        // -------------------------------------------
        TimeSpan GetRemindLaterInterval(ushort interval) => TimerDurationTimeSpan switch
        {
            RemindLaterFormat.Seconds => TimeSpan.FromSeconds(interval),
            RemindLaterFormat.Minutes => TimeSpan.FromMinutes(interval),
            RemindLaterFormat.Hours   => TimeSpan.FromHours(interval),
            RemindLaterFormat.Days    => TimeSpan.FromDays(interval),
            RemindLaterFormat.Weeks   => TimeSpan.FromDays(interval * 7),
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
    private void Cancel() => Copy(_configOrig);


    [RelayCommand]
    public override void Update()
    {
        SaveConfig();

        _Update();
    }


    private void _Update()
    {
        SetVersion();

        _configOrig.Copy(this);

        _UpdateTitle();
        _UpdateIcon(TmpIcon);
        _UpdateValidation(false);
    }


    [RelayCommand]
    private void SaveConfig()
    {
        var directory = $@"{Directory.GetCurrentDirectory()}\Properties";
        var file      = $@"{directory}\{Settings.Default!.ConfigFile}";

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
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            var json = JsonSerializer.Serialize<ViewModelMainConfig>(this, _jso);
            File.WriteAllTextAsync(file, json);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }
    }


    [RelayCommand]
    private void LoadConfig()
    {
        var file = $@"{Directory.GetCurrentDirectory()}\Properties\{Settings.Default!.ConfigFile}";
        if (!File.Exists(file))
            return;

        var json = File.ReadAllText(file);

        try
        {
            var vmmc = JsonSerializer.Deserialize<ViewModelMainConfig>(json, _jso);
            Copy(vmmc);

            _Update();
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
        var uri = new Uri(imageUri, imageUri.StartsWith("pack:") ? UriKind.Absolute : UriKind.Relative);

        TmpIcon = uri.ConvertToBitmapImage();

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