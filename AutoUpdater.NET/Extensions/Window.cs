// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Window.cs
// Author:   Latency McLaughlin
// Date:     01/09/2026
// ****************************************************************************

using System.Windows;

namespace AutoUpdaterDotNET.Extensions;

public static class WindowExtensions
{
    /// <summary>
    ///     Show the window instance.
    /// </summary>
    /// <param name="window">The host builder to configure with automatic update services. Cannot be null.</param>
    /// <param name="isDialog"></param>
    /// <returns>None</returns>
    public static void Show(this Window window, bool isDialog = false)
    {
        if (isDialog)
            window.ShowDialog();
        else
            window.Show();
    }
}