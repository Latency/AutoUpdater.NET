// ****************************************************************************
// Project:  Patch1
// File:     ViewModelRemindLater.cs
// Author:   Latency McLaughlin
// Date:     05/03/2024
// ****************************************************************************
// ReSharper disable InconsistentNaming

using System.IO;
using System.Windows;
using System.Windows.Input;
using AutoUpdaterDotNET.Views;
using AutoUpdaterDotNET.Commands;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.ViewModels;

public class ViewModelRemindLater : DependencyObject, IViewModelRemindLater
{
    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    private static bool AllowPatch(object? _) => true;

    public ICommand CommandPatch   { get; }
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Fields
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Fields


    /// <summary>
    ///     Constructor
    /// </summary>
    public ViewModelRemindLater()
    {
        CommandPatch = new RelayCommand(TransButtonPatch_Click, AllowPatch);

        string? folder;
        using (var myKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Audials\RSConfig\VCDWriter", false))
        {
            var path = myKey?.GetValue("LaunchExe") as string;
            folder = Path.GetDirectoryName(path);
        }

        InstallationFolder = folder ?? throw new NullReferenceException("Installation is missing or corrupted!");
        Version            = ushort.Parse(new string(InstallationFolder.Split('\\').Last().SkipWhile(c => c != ' ').Skip(1).ToArray()));
        ReleaseDate        = File.GetCreationTime($@"{InstallationFolder}\{Environment.GetEnvironmentVariable("Assembly Name")}").ToShortDateString();
    }


    internal string InstallationFolder { get; }
    internal ushort Version            { get; }
    internal string ReleaseDate        { get; }


    // ReSharper disable once AsyncVoidMethod
    public void TransButtonPatch_Click(object? sender)
    {
        if (sender is not Window_Main win)
            throw new NullReferenceException();

    }
}