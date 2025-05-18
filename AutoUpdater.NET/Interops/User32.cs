// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     User32.cs
// Author:   Latency McLaughlin
// Date:     05/18/2025
// ****************************************************************************

using System.Runtime.InteropServices;

namespace AutoUpdaterDotNET.Interops;

internal static class User32
{
    [DllImport("user32.dll")]
    internal static extern nint GetWindowLongPtr(nint hWnd, int nIndex);

    [DllImport("user32.dll")]
    internal static extern nint SetWindowLongPtr(nint hWnd, int nIndex, IntPtr dwNewLong);
}