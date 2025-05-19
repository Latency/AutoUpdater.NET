// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     05/18/2025
// ****************************************************************************

using System.Windows;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.ViewModels;

public sealed class ViewModelDownloadUpdate : DependencyObject, IViewModelDownloadUpdate
{
    public static readonly DependencyProperty ProgressPercentageProperty = DependencyProperty.Register(nameof(ProgressPercentage), typeof(double), typeof(ViewModelDownloadUpdate), new PropertyMetadata(0.0));


    /// <summary>
    ///     Constructor
    /// </summary>
    public ViewModelDownloadUpdate()
    {
    }


    public double ProgressPercentage
    {
        get => (double)GetValue(ProgressPercentageProperty);
        set => SetValue(ProgressPercentageProperty, value);
    }
}