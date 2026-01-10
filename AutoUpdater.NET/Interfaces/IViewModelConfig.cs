// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Windows.Threading;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelConfig : IViewModelMainConfig
{
    DispatcherTimer UpdateTimer { get; }
}