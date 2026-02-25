// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Encoding2.cs
// Author:   Latency McLaughlin
// Date:     02/24/2026
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Text;

namespace AutoUpdaterDotNET.Models;

public partial class Encoding2 : ObservableObject
{
    private readonly Encoding _encoding = new UTF8Encoding();


    /// <summary>
    ///     Default Constructor
    /// </summary>
    public Encoding2()
    { }


    /// <summary>
    ///     Copy Constructor
    /// </summary>
    /// <param name="encoding"></param>
    public Encoding2(Encoding encoding) : this()
    {
        _encoding = encoding;

        BodyName          = encoding.BodyName;
        CodePage          = encoding.CodePage;
        DecoderFallback   = encoding.DecoderFallback;
        EncoderFallback   = encoding.EncoderFallback;
        EncodingName      = encoding.EncodingName;
        HeaderName        = encoding.HeaderName;
        IsBrowserDisplay  = encoding.IsBrowserDisplay;
        IsBrowserSave     = encoding.IsBrowserSave;
        IsMailNewsDisplay = encoding.IsMailNewsDisplay;
        IsMailNewsSave    = encoding.IsMailNewsSave;
        IsReadOnly        = encoding.IsReadOnly;
        IsSingleByte      = encoding.IsSingleByte;
        WebName           = encoding.WebName;
        WindowsCodePage   = encoding.WindowsCodePage;
    }

    public static explicit operator Encoding2(Encoding encoder) => new(encoder);
    public static implicit operator Encoding(Encoding2 encoder) => encoder._encoding;


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Name")]
    [Description("Returns the name for this encoding that can be used with mail agent body tags.  If the encoding may not be used, the string is empty.")]
    public partial string? BodyName { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Code Page")]
    public partial int CodePage { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Encoding Name")]
    [Description("Returns the human-readable description of the encoding.")]
    public partial string? EncodingName { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Header Name")]
    [Description("Returns the name for this encoding that can be used with mail agent header tags.  If the encoding may not be used, the string is empty.")]
    [Browsable(false)]
    public partial string? HeaderName { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [Browsable(false)]
    [DisplayName("Web Name")]
    [Description("Returns the IANA preferred name for this encoding.")]
    public partial string? WebName { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Windows Code Page")]
    [Description("Returns the windows code page that most closely corresponds to this encoding.")]
    public partial int WindowsCodePage { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Browser Display")]
    [Description("True if and only if the encoding is used for display by browsers clients.")]
    public partial bool IsBrowserDisplay { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Browser Save")]
    [Description("True if and only if the encoding is used for saving by browsers clients.")]
    public partial bool IsBrowserSave { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Main News Display")]
    [Description("True if and only if the encoding is used for display by mail and news clients.")]
    public partial bool IsMailNewsDisplay { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Mail News Save")]
    [Description("True if and only if the encoding is used for saving documents by mail and news clients")]
    public partial bool IsMailNewsSave { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Single Byte")]
    [Description("True if and only if the encoding only uses single byte code points.")]
    public partial bool IsSingleByte { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Read Only")]
    public partial bool IsReadOnly { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Encoder Fallback")]
    [Browsable(false)]
    public partial EncoderFallback? EncoderFallback { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Decoder Fallback")]
    [Browsable(false)]
    public partial DecoderFallback? DecoderFallback { get; set; }
}