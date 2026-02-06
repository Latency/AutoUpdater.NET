// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IViewModelMainConfig.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Reflection;

namespace AutoUpdaterDotNET.Interfaces;

public interface IViewModelConfig : IViewModelMainConfig, IViewModelRestricted
{
    Task Start(string domain, Assembly? myAssembly = null);
}