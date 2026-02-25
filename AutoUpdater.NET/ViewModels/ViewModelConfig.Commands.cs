// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelConfig.Commands.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Extensions;
using AutoUpdaterDotNET.Models;
using AutoUpdaterDotNET.Properties;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig
{
    [RelayCommand]
    private void Loaded() => LoadConfig();


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
        _configOrig.Copy(_config);

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
            var vmmc = JsonSerializer.Deserialize<Config>(json, _jso);
            if (vmmc is null)
                throw new NullReferenceException("Unable to deserialize json from 'Config'.");

            // Preserve Properties
            vmmc.EqualsPredicate               = _config.EqualsPredicate;
            vmmc.BeforeCheckForUpdatesNodeList = _config.BeforeCheckForUpdatesNodeList;
            vmmc.AfterCheckForUpdatesNodeList  = _config.AfterCheckForUpdatesNodeList;
            vmmc.TimerNodeList                 = _config.TimerNodeList;
            vmmc.UpdateCompleteNodeList        = _config.UpdateCompleteNodeList;

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