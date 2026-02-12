// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Reflection;
using WindowService.Interfaces;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelConfig : IViewModelRestricted
{
    IConfig Config { get; }

    Task Start(string domain, Assembly? myAssembly = null);
}