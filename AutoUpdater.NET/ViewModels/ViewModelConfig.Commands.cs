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
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace AutoUpdaterDotNET.ViewModels;

public partial class ViewModelConfig
{
    [RelayCommand]
    private void Loaded() => LoadConfig();


    [RelayCommand]
    private void Cancel() => Config = new Config(_configOrig);


    [RelayCommand]
    private void Update()
    {
        SaveConfig();
        _Update();
    }

    private void _Update(bool init=false)
    {
        if (!init)
            _configOrig = new Config(Config);

        var _config = (Config)Config;
        _config._UpdateVersion();
        _config._UpdateTitle();
        _config._UpdateIcon(Config.TmpIcon);
        _config._UpdateValidation(false);
    }


    [RelayCommand]
    private void SaveConfig()
    {
        var directory = $@"{Directory.GetCurrentDirectory()}\Properties";
        // TODO:
        // Change to embedded resource
        var file = $@"{directory}\{Settings.Default!.ConfigFile!.Decrypt(Settings.Default.CipherKey!)}";

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

        //    if (File.Exists(file) && File.OpenRead(file).Length == 0)Z
        //        File.Delete(file);
        //    return;

        //}

        try
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var obj = ((Config) Config).Clone() as Config;

            if (obj.FtpProfile is not null && (obj.FtpProfile.Credentials is not null || obj.FtpProfile.Encoding is not null))
            {
                if (obj.FtpProfile.Credentials is not null && obj.FtpProfile.Credentials.IsDefault())
                    obj.FtpProfile.Credentials = null;
                if (obj.FtpProfile.Encoding is not null && obj.FtpProfile.Encoding.IsDefault())
                    obj.FtpProfile.Encoding = null;
            }

            if (obj.InstallationPathOverride is not null && string.IsNullOrEmpty(obj.InstallationPathOverride.Path))
                obj.InstallationPathOverride = null;

            if (obj.WindowSize is not null && obj.WindowSize.IsDefault())
                obj.WindowSize = null;

            var json = JsonSerializer.Serialize(obj, _jso);
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
        // TODO:
        // Change to embedded resource
        var file = $@"{Directory.GetCurrentDirectory()}\Properties\{Settings.Default!.ConfigFile!.Decrypt(Settings.Default.CipherKey!)}";

        if (!File.Exists(file))
            return;

        var json = File.ReadAllText(file);

        try
        {
            var obj = JsonSerializer.Deserialize<Config>(json, _jso) ?? Error()!;

            // Seconds is not allowed as a TimeSpan
            if (obj.RemindLaterTimer is not null && obj.RemindLaterTimer.TimeSpan == RemindLaterFormat.Seconds)
                obj.RemindLaterTimer.TimeSpan = RemindLaterFormat.Minutes;

            // Special handling for loading Encoding
            if (obj.FtpProfile?.Encoding is not null)
                obj.FtpProfile.Encoding = new Encoding2(obj.FtpProfile.Encoding);

            var _config = (Config)Config;
            _config.Clone(obj);
            _configOrig = new(Config);
            _config.EqualsPredicate = () => _configOrig.Equals(Config);

            // Event Invocator
            Register?.Invoke(_config);

            _Update(init: true);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
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
        Config.IconOverride!.Uri = new Uri(imageUri, imageUri.StartsWith("pack:") ? UriKind.Absolute : UriKind.Relative);
        Config.TmpIcon = Config.IconOverride.Uri.ConvertToBitmapImage();

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