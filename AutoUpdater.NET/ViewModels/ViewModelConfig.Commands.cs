// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.Commands.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AssemblyLoader;
using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Properties;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig
{
    [RelayCommand]
    private void Loaded() => LoadConfig();


    [RelayCommand]
    private void Cancel() => _config = new(_configOrig);


    [RelayCommand]
    private void Update()
    {
        SaveConfig();
        _Update();
    }


    private void _Update(bool init=false)
    {
        if (!init)
            _configOrig = new(_config);

        _config._UpdateVersion();
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
        var directory = $@"{Directory.GetCurrentDirectory()}\Properties";
        var file      = $@"{directory}\{Settings.Default!.ConfigFile}";

        if (!File.Exists(file))
            return;

        var json = File.ReadAllText(file);

        try
        {
            var obj = JsonSerializer.Deserialize<Config>(json, _jso) ?? Error()!;
            _config.Clone(obj);

            // Preserve Properties

            //vmmc.BeforeCheckForUpdatesNodeList = _config.BeforeCheckForUpdatesNodeList;
            //vmmc.AfterCheckForUpdatesNodeList  = _config.AfterCheckForUpdatesNodeList;
            //vmmc.TimerNodeList                 = _config.TimerNodeList;
            //vmmc.UpdateCompleteNodeList        = _config.UpdateCompleteNodeList;

            _config.InstalledVersionOverride = obj.InstalledVersion != null;
            _config.WindowSizeOverride       = obj.WindowSize       != null;
            _config.EqualsPredicate          = () => _configOrig.Equals(_config);

            try
            {
                _configOrig = new(_config)
                {
                    EqualsPredicate = null
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _config.InstalledVersion     ??= new(GetType().Assembly.Version()!);
            _configOrig.InstalledVersion ??= new(_config.InstalledVersion);

            _config.WindowSize     ??= new();
            _configOrig.WindowSize ??= new(_config.WindowSize);

            _config.TmpIcon     ??= _config.IconOverride?.Uri != null ? _config.IconOverride.Uri.ConvertToBitmapImage() : Application.Current!.FindResource("project") as BitmapImage;
            _configOrig.TmpIcon ??= _config.TmpIcon!.Clone();

            // Event Invocator
            Register?.Invoke(_config);

            _Update(init: true);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }

        return;

        static Config? Error()
        {
            MessageBox.Show("Unable to deserialize json from 'Config'.", "Configuration Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current!.Shutdown();
            return null;
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