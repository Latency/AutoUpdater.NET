// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IDownload.cs
// Author:   Latency McLaughlin
// Date:     02/20/2026
// ****************************************************************************

using AutoUpdaterDotNET.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Windows.Controls;

namespace AutoUpdaterDotNET.Interfaces;

internal interface IDownload
{
    event EventHandler       UpdateTimer;
    event Action             UpdateComplete;
    event Action             BeforeCheckForUpdates;
    event Action<UpdateInfo> AfterCheckForUpdates;

    ObservableCollection<TreeViewItem> TimerNodeList                 { get; init; }
    ObservableCollection<TreeViewItem> UpdateCompleteNodeList        { get; init; }
    ObservableCollection<TreeViewItem> BeforeCheckForUpdatesNodeList { get; init; }
    ObservableCollection<TreeViewItem> AfterCheckForUpdatesNodeList  { get; init; }

    IConfig Config { get; init; }

    // ReSharper disable once InconsistentNaming
    CancellationTokenSource? CTS { get; }

    bool IsRunning { get; set; }

    AuthenticationHeaderValue? BasicAuthHeaderValue { get; set; }

    Task Start();
}