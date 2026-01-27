// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using AutoUpdaterDotNET.Models;
using System.Reflection;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelConfig : IViewModelMainConfig
{
    void ShowUpdateForm(UpdateInfoEventArgs args);
    Task Start(string domain, Assembly? myAssembly = null);
}