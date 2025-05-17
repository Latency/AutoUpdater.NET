// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     RegistryPersistenceProvider.cs
// Author:   Latency McLaughlin
// Date:     05/16/2025
// ****************************************************************************

using System.Globalization;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Persistance_Providers;

/// <summary>
///     Provides a mechanism for storing AutoUpdater state between sessions based on storing data on the Windows Registry.
/// </summary>
public class Registry : IPersistenceProvider
{
    private const string RemindLaterValueName    = "RemindLaterAt";
    private const string SkippedVersionValueName = "SkippedVersion";

    /// <summary>
    ///     Initializes a new instance of the RegistryPersistenceProvider class indicating the path for the Windows registry
    ///     key to use for storing the data.
    /// </summary>
    /// <param name="registryLocation"></param>
    public Registry(string registryLocation) => RegistryLocation = registryLocation;


    /// <summary>
    ///     Gets/sets the path for the Windows Registry key that will contain the data.
    /// </summary>
    private string RegistryLocation { get; }

    /// <inheritdoc />
    public Version? GetSkippedVersion()
    {
        try
        {
            using var updateKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryLocation);
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
        using var updateKey        = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryLocation);
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
        using var autoUpdaterKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistryLocation);
        autoUpdaterKey.SetValue(SkippedVersionValueName, version != null ? version.ToString() : string.Empty);
    }


    /// <inheritdoc />
    public void SetRemindLater(DateTime? remindLaterAt)
    {
        using var autoUpdaterKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistryLocation);
        autoUpdaterKey.SetValue(RemindLaterValueName, remindLaterAt != null ? remindLaterAt.Value.ToString(CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat) : string.Empty);
    }
}