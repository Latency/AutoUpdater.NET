// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Config.cs
// Author:   Latency McLaughlin
// Date:     06/18/2025
// ****************************************************************************

using AutoUpdaterDotNET.Enums;
using System.ComponentModel.DataAnnotations;
using AutoUpdaterDotNET.Interfaces;

namespace AutoUpdaterDotNET.Models;

public record Config : IConfig
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="c"></param>
    public Config(Config c)
    {
        ProxyUri                      = c.ProxyUri;
        IsManditory                   = c.IsManditory;
        ShowSkipButton                = c.ShowSkipButton;
        ShowRemindLaterButton         = c.ShowRemindLaterButton;
        RunUpdateAsAdmin              = c.RunUpdateAsAdmin;
        OpenDownloadPage              = c.OpenDownloadPage;
        LetUserSelectRemindLater      = c.LetUserSelectRemindLater;
        RemindLaterTimeSpan           = c.RemindLaterTimeSpan;
        TimerDurationTimeSpan         = c.TimerDurationTimeSpan;
        RemindLaterAt                 = c.RemindLaterAt;
        AppTitle                      = c.AppTitle;
        ReportErrors                  = c.ReportErrors;
        TimerEnabled                  = c.TimerEnabled;
        Interval                      = c.Interval;
        ProxyEnabled                  = c.ProxyEnabled;
        ProxyUsername                 = c.ProxyUsername;
        ProxyPassword                 = c.ProxyPassword;
        UpdateMode                    = c.UpdateMode;
        BasicAuth                     = c.BasicAuth;
        BasicAuthDownload             = c.BasicAuthDownload;
        BasicAuthChangeLog            = c.BasicAuthChangeLog;
        BasicAuthUsername             = c.BasicAuthUsername;
        BasicAuthPassword             = c.BasicAuthPassword;
        FtpProtocol                   = c.FtpProtocol;
        PersistSettings               = c.PersistSettings;
        UseZipFile                    = c.UseZipFile;
        ChangeUpdateZipExtractionPath = c.ChangeUpdateZipExtractionPath;
        InstallationPath              = c.InstallationPath;
        CheckSynchronously            = c.CheckSynchronously;
        InstalledVersionOverride      = c.InstalledVersionOverride;
        MajorVersion                  = c.MajorVersion;
        MinorVersion                  = c.MinorVersion;
        SubPatchVersion               = c.SubPatchVersion;
        BuildVersion                  = c.BuildVersion;
        ClearAppDirectory             = c.ClearAppDirectory;
        ExecutablePathOverride        = c.ExecutablePathOverride;
        ExecutablePath                = c.ExecutablePath;
        BindOwnerWindow               = c.BindOwnerWindow;
        TopMostEnabled                = c.TopMostEnabled;
        IconOverride                  = c.IconOverride;
        ImageUri                      = c.ImageUri;
    }

    [Url]
    public string?           ProxyUri                      { get; set; }
    public bool              IsManditory                   { get; set; }
    public bool              ShowSkipButton                { get; set; }
    public bool              ShowRemindLaterButton         { get; set; }
    public bool              RunUpdateAsAdmin              { get; set; }
    public bool              OpenDownloadPage              { get; set; }
    public bool              LetUserSelectRemindLater      { get; set; }
    public RemindLaterFormat RemindLaterTimeSpan           { get; set; }
    public RemindLaterFormat TimerDurationTimeSpan         { get; set; }
    public ushort            RemindLaterAt                 { get; set; }
    public string?           AppTitle                      { get; set; }
    public bool              ReportErrors                  { get; set; }
    public bool              TimerEnabled                  { get; set; }
    public ushort            Interval                      { get; set; }
    public bool              ProxyEnabled                  { get; set; }
    public string?           ProxyUsername                 { get; set; }
    public string?           ProxyPassword                 { get; set; }
    public Mode              UpdateMode                    { get; set; }
    public bool              BasicAuth                     { get; set; }
    public bool              BasicAuthDownload             { get; set; }
    public bool              BasicAuthChangeLog            { get; set; }
    public string?           BasicAuthUsername             { get; set; }
    public string?           BasicAuthPassword             { get; set; }
    public bool              FtpProtocol                   { get; set; }
    public bool              PersistSettings               { get; set; }
    public bool              UseZipFile                    { get; set; }
    public bool              ChangeUpdateZipExtractionPath { get; set; }
    public string?           InstallationPath              { get; set; }
    public bool              CheckSynchronously            { get; set; }
    public bool              InstalledVersionOverride      { get; set; }
    public ushort            MajorVersion                  { get; set; } = 1;
    public ushort            MinorVersion                  { get; set; }
    public ushort            SubPatchVersion               { get; set; }
    public ushort            BuildVersion                  { get; set; }
    public bool              ClearAppDirectory             { get; set; }
    public bool              ExecutablePathOverride        { get; set; }
    public string?           ExecutablePath                { get; set; }
    public bool              BindOwnerWindow               { get; set; } = true;
    public bool              TopMostEnabled                { get; set; } = true;
    public bool              IconOverride                  { get; set; }
    public string?           ImageUri                      { get; set; }
}