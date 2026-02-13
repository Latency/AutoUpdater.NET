// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.Commands.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Enums;
using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Properties;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Controls;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig
{
    [RelayCommand]
    private void Loaded()
    {
        _updateTimer.Interval = GetRemindLaterInterval(_config.TimerInterval);

        InvocationListGenerator(_updateTimer, nameof(_updateTimer.Tick), _config.TimerNodeList);
        InvocationListGenerator(this,         nameof(UpdateComplete),    _config.UpdateCompleteNodeList);
        InvocationListGenerator(this,         nameof(CheckForUpdates),   _config.CheckForUpdatesNodeList);

        LoadConfig();

        return;

        // -------------------------------------------
        TimeSpan GetRemindLaterInterval(ushort interval) => _config.TimerDurationTimeSpan switch
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
    private void Cancel() => _config.Copy(_configOrig);


    [RelayCommand]
    private void Update()
    {
        SaveConfig();
        _Update();
    }


    private void _Update()
    {
        _config.SetVersion();

        _configOrig.Copy(_config);

        _config._UpdateTitle();
        _config._UpdateIcon(_config.TmpIcon);
        _config._UpdateValidation(false);
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
            var json = JsonSerializer.Serialize(_config, _jso);
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
            var vmmc = JsonSerializer.Deserialize<Config>(json, _jso);
            if (vmmc is null)
                throw new NullReferenceException("Unable to deserialize json from 'Config'.");

            // Preserve Properties
            vmmc.EqualsPredicate         = _config.EqualsPredicate;
            vmmc.CheckForUpdatesNodeList = _config.CheckForUpdatesNodeList;
            vmmc.TimerNodeList           = _config.TimerNodeList;
            vmmc.UpdateCompleteNodeList  = _config.UpdateCompleteNodeList;

            _config.Copy(vmmc);

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
        _config.IconOverride!.Uri = new Uri(imageUri, imageUri.StartsWith("pack:") ? UriKind.Absolute : UriKind.Relative);
        _config.TmpIcon = _config.IconOverride.Uri.ConvertToBitmapImage();

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