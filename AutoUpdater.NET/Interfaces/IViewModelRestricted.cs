// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelRestricted.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Windows;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelRestricted
{
    public IViewModelMainConfig?   Config { get; set; }
    public IFrameworkInputElement? Owner  { get; set; } // Window
}