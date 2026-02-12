// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window_DownloadUpdate.xaml.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************
// ReSharper disable InconsistentNaming

using AutoUpdaterDotNET.Interfaces;
using AutoUpdaterDotNET.Properties;
using AutoUpdaterDotNET.ViewModels;
using System.Globalization;
using System.Windows;

namespace AutoUpdaterDotNET.Views;

public sealed partial class Window_DownloadUpdate
{
    private DateTime _startedAt;


    /// <summary>
    ///     Constructor
    /// </summary>
    public Window_DownloadUpdate()
    {
        InitializeComponent();
    }


    private void Window_Restricted_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not IViewModelDownloadUpdate vmDownloadUpdate)
            throw new NullReferenceException();

        vmDownloadUpdate.ProgressBarCallback = OnProgressBarChanged;
        vmDownloadUpdate.ContentCallback     = OnContentChanged;

        ViewModelDownloadUpdate.Wnd_Loaded(this);
    }


    private void OnProgressBarChanged(double value)
    {
        // This code runs on the UI thread, allowing safe updates to the ProgressBar
        ProgressBarDownload?.Value = value;

        // Optional: Update the UI immediately to avoid display delays
        ProgressBarDownload?.UpdateLayout();
    }


    private void OnContentChanged(long bytesReceived, long totalBytesToReceive)
    {
        if (_startedAt == default)
            _startedAt = DateTime.Now;
        else
        {
            var timeSpan     = DateTime.Now - _startedAt;
            var totalSeconds = (long)timeSpan.TotalSeconds;
            if (totalSeconds > 0)
            {
                var bytesPerSecond = bytesReceived / totalSeconds;
                LabelInformation?.Content = string.Format(Settings.Default!.DownloadSpeedMessage!, BytesToString(bytesPerSecond));
            }
        }

        LabelSize?.Content = $"{BytesToString(bytesReceived)} / {BytesToString(totalBytesToReceive)}";

        return;

        static string BytesToString(long byteCount)
        {
            string[] suf = ["B", "KB", "MB", "GB", "TB", "PB", "EB"];
            if (byteCount == 0)
                return "0" + suf[0];

            var bytes = Math.Abs(byteCount);
            var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            var num   = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
        }
    }
}