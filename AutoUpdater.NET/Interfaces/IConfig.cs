// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IConfig.cs
// Author:   Latency McLaughlin
// Date:     06/18/2025
// ****************************************************************************

using System.ComponentModel.DataAnnotations;
using AutoUpdaterDotNET.Enums;

namespace AutoUpdaterDotNET.Interfaces;

public interface IConfig
{
    [Url]
    string?           ProxyUri                      { get; set; }
    bool              IsManditory                   { get; set; }
    bool              ShowSkipButton                { get; set; }
    bool              ShowRemindLaterButton         { get; set; }
    bool              RunUpdateAsAdmin              { get; set; }
    bool              OpenDownloadPage              { get; set; }
    bool              LetUserSelectRemindLater      { get; set; }
    RemindLaterFormat RemindLaterTimeSpan           { get; set; }
    RemindLaterFormat TimerDurationTimeSpan         { get; set; }
    ushort            RemindLaterAt                 { get; set; }
    string?           AppTitle                      { get; set; }
    bool              ReportErrors                  { get; set; }
    bool              TimerEnabled                  { get; set; }
    ushort            Interval                      { get; set; }
    bool              ProxyEnabled                  { get; set; }
    string?           ProxyUsername                 { get; set; }
    string?           ProxyPassword                 { get; set; }
    Mode              UpdateMode                    { get; set; }
    bool              BasicAuth                     { get; set; }
    bool              BasicAuthDownload             { get; set; }
    bool              BasicAuthChangeLog            { get; set; }
    string?           BasicAuthUsername             { get; set; }
    string?           BasicAuthPassword             { get; set; }
    bool              FtpProtocol                   { get; set; }
    bool              PersistSettings               { get; set; }
    bool              UseZipFile                    { get; set; }
    bool              ChangeUpdateZipExtractionPath { get; set; }
    string?           InstallationPath              { get; set; }
    bool              CheckSynchronously            { get; set; }
    bool              InstalledVersionOverride      { get; set; }
    ushort            MajorVersion                  { get; set; }
    ushort            MinorVersion                  { get; set; }
    ushort            SubPatchVersion               { get; set; }
    ushort            BuildVersion                  { get; set; }
    bool              ClearAppDirectory             { get; set; }
    bool              ExecutablePathOverride        { get; set; }
    string?           ExecutablePath                { get; set; }
    bool              BindOwnerWindow               { get; set; }
    bool              TopMostEnabled                { get; set; }
    bool              IconOverride                  { get; set; }
    string?           ImageUri                      { get; set; }
}