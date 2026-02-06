// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     IWindowService.cs
// Author:   Latency McLaughlin
// Date:     01/07/2026
// ****************************************************************************

using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace AutoUpdaterDotNET.Interfaces;

public interface IWindowService : IAddChild, IFrameworkInputElement, ISupportInitialize, IQueryAmbient, IAnimatable
{
    ResourceDictionary Resources { get; set; }

    /// <summary>
    /// The data that will be displayed as the title of the window.  Hosts 
    /// are free to display the title in any manner that they want.  For 
    /// example, the browser may display the title set via the Title 
    /// property somewhere besides the caption bar.
    /// </summary>
    string? Title { get; set; }

    /// <summary>
    /// Height of the host window
    /// </summary>
    double Height { get; set; }

    /// <summary>
    /// Width of the host window
    /// </summary>
    double Width { get; set; }

    bool UserResized { get; }


    TWindow InitializeWindow<TWindow, TViewModel>(Window? owner)
        where TWindow : Window
        where TViewModel : notnull;

    void Show();

    void Hide();

    bool? ShowDialog();
}