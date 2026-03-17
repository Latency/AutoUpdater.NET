// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Encoding2.cs
// Author:   Latency McLaughlin
// Date:     02/24/2026
// ****************************************************************************

using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using AutoUpdaterDotNET.Attributes;

namespace AutoUpdaterDotNET.Models;

public partial class Encoding2 : ObservableObject, IEquatable<Encoding2>
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
    [JsonPropertyName("Name")]
    [JsonComment("Only standard encodings are supported!\nus-ascii\nutf-16BE\niso-8859-1\nutf-8\nutf-16\nutf-32")]
    public partial string? BodyName { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Code Page")]
    [JsonIgnore]
    public partial int CodePage { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Encoding Name")]
    [Description("Returns the human-readable description of the encoding.")]
    [JsonIgnore]
    public partial string? EncodingName { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Header Name")]
    [Description("Returns the name for this encoding that can be used with mail agent header tags.  If the encoding may not be used, the string is empty.")]
    [Browsable(false)]
    [JsonIgnore]
    public partial string? HeaderName { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [Browsable(false)]
    [DisplayName("Web Name")]
    [Description("Returns the IANA preferred name for this encoding.")]
    [JsonIgnore]
    public partial string? WebName { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Windows Code Page")]
    [Description("Returns the windows code page that most closely corresponds to this encoding.")]
    [JsonIgnore]
    public partial int WindowsCodePage { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Browser Display")]
    [Description("True if and only if the encoding is used for display by browsers clients.")]
    [JsonIgnore]
    public partial bool IsBrowserDisplay { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Browser Save")]
    [Description("True if and only if the encoding is used for saving by browsers clients.")]
    [JsonIgnore]
    public partial bool IsBrowserSave { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Main News Display")]
    [Description("True if and only if the encoding is used for display by mail and news clients.")]
    [JsonIgnore]
    public partial bool IsMailNewsDisplay { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Mail News Save")]
    [Description("True if and only if the encoding is used for saving documents by mail and news clients")]
    [JsonIgnore]
    public partial bool IsMailNewsSave { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Single Byte")]
    [Description("True if and only if the encoding only uses single byte code points.")]
    [JsonIgnore]
    public partial bool IsSingleByte { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Is Read Only")]
    [JsonIgnore]
    public partial bool IsReadOnly { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Encoder Fallback")]
    [Browsable(false)]
    [JsonIgnore]
    public partial EncoderFallback? EncoderFallback { get; set; }


    [ObservableProperty]
    [Category("Encoding")]
    [ReadOnly(true)]
    [DisplayName("Decoder Fallback")]
    [Browsable(false)]
    [JsonIgnore]
    public partial DecoderFallback? DecoderFallback { get; set; }


    public bool Equals(Encoding2? other)
    {
        return
            other is not null                             &&
            BodyName          == other.BodyName           &&
            CodePage          == other.CodePage           &&
            EncodingName      == other.EncodingName       &&
            HeaderName        == other.HeaderName         &&
            WebName           == other.WebName            &&
            WindowsCodePage   == other.WindowsCodePage    &&
            IsBrowserDisplay  == other.IsBrowserDisplay   &&
            IsBrowserSave     == other.IsBrowserSave      &&
            IsMailNewsDisplay == other.IsMailNewsDisplay  &&
            IsMailNewsSave    == other.IsMailNewsSave     &&
            IsSingleByte      == other.IsSingleByte       &&
            IsReadOnly        == other.IsReadOnly;
    }


    public override int GetHashCode() => HashCode.Combine(BodyName, CodePage, EncodingName, HeaderName, WebName, WindowsCodePage);
}