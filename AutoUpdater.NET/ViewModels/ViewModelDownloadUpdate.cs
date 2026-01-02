// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     ViewModelDownloadUpdate.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoUpdaterDotNET.ViewModels;

public sealed partial class ViewModelDownloadUpdate : ObservableObject
{
    [ObservableProperty]
    public partial double ProgressPercentage { get; set; }
}