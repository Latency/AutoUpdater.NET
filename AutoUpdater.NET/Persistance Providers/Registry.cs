// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Registry.cs
// Author:   Latency McLaughlin
// Date:     06/10/2025
// ****************************************************************************

using System.Globalization;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Persistance_Providers;

/// <summary>
///     Provides a mechanism for storing AutoUpdater state between sessions based on storing data on the Windows Registry.
/// </summary>
/// <remarks>
///     Initializes a new instance of the RegistryPersistenceProvider class indicating the path for the Windows registry
///     key to use for storing the data.
/// </remarks>
/// <param name="registryLocation"></param>
public class Registry(string registryLocation) : IPersistenceProvider
{
    private const string RemindLaterValueName    = "RemindLaterAt";
    private const string SkippedVersionValueName = "SkippedVersion";


    /// <inheritdoc />
    public Version? GetSkippedVersion()
    {
        try
        {
            using var updateKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(registryLocation);
            if (updateKey?.GetValue(SkippedVersionValueName) is string skippedVersionValue)
                return new Version(skippedVersionValue);
        }
        catch (Exception)
        {
            // ignored
        }

        return null;
    }


    /// <inheritdoc />
    public DateTime? GetRemindLater()
    {
        using var updateKey        = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(registryLocation);
        var       remindLaterValue = updateKey?.GetValue(RemindLaterValueName);

        if (remindLaterValue == null)
            return null;

        try
        {
            return Convert.ToDateTime(remindLaterValue.ToString(), CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat);
        }
        catch (FormatException)
        {
            // ignored
        }

        return null;
    }


    /// <inheritdoc />
    public void SetSkippedVersion(Version? version)
    {
        using var autoUpdaterKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(registryLocation);
        autoUpdaterKey.SetValue(SkippedVersionValueName, version != null ? version.ToString() : string.Empty);
    }


    /// <inheritdoc />
    public void SetRemindLater(DateTime? remindLaterAt)
    {
        using var autoUpdaterKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(registryLocation);
        autoUpdaterKey.SetValue(RemindLaterValueName, remindLaterAt != null ? remindLaterAt.Value.ToString(CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat) : string.Empty);
    }
}