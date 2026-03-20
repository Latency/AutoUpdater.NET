// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Encoding2.cs
// Author:   Latency McLaughlin
// Date:     02/24/2026
// ****************************************************************************

using AutoUpdaterDotNET.Attributes;
using AutoUpdaterDotNET.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace AutoUpdaterDotNET.Models;

public partial class Encoding2 : ObservableObject, IEquatable<Encoding2>
{
    #region Constructors
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    /// <summary>
    ///     Default Constructor
    /// </summary>
    public Encoding2()
    { }


    /// <summary>
    ///     Copy Constructor (Overload +1)
    /// </summary>
    /// <param name="encoding"></param>
    public Encoding2(Encoding encoding) : this()
    {
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


    /// <summary>
    ///     Copy Constructor (Overload +2)
    /// </summary>
    /// <param name="encodings"></param>
    public Encoding2(Encodings encodings) : this(ToEncoding(encodings))
    { }


    /// <summary>
    ///     Copy Constructor (Overload +3)
    /// </summary>
    /// <param name="name"></param>
    public Encoding2(string name) : this(ToEncoding(name))
    { }


    public static explicit operator Encoding2(Encoding encoder) => new(encoder);
    public static implicit operator Encoding(Encoding2 encoder) => ToEncoding(encoder.BodyName!);

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Constructors

    #region Properties
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

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

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Properties


    #region Methods
    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

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

    public override bool Equals(object? obj) => Equals(obj as Encoding2);

    public static Encoding ToEncoding(Encodings encodings) => encodings switch
    {
        Encodings.ASCII            => Encoding.ASCII,
        Encodings.BigEndianUnicode => Encoding.BigEndianUnicode,
        Encodings.Latin1           => Encoding.Latin1,
        Encodings.UTF32            => Encoding.UTF32,
        Encodings.UTF8             => Encoding.UTF8,
        Encodings.Unicode          => Encoding.Unicode,
        _                          => Encoding.Default
    };

    public static Encoding ToEncoding(string name) => name switch
    {
        "us-ascii"   => Encoding.ASCII,
        "utf-16BE"   => Encoding.BigEndianUnicode,
        "iso-8859-1" => Encoding.Latin1,
        "utf-32"     => Encoding.UTF32,
        "utf-8"      => Encoding.UTF8,
        "utf-16"     => Encoding.Unicode,
        _            => Encoding.Default
    };

    public static Encodings ToEncodings(string name) => name switch
    {
        "us-ascii"   => Encodings.ASCII,
        "utf-16BE"   => Encodings.BigEndianUnicode,
        "iso-8859-1" => Encodings.Latin1,
        "utf-8"      => Encodings.UTF8,
        "utf-16"     => Encodings.Unicode,
        "utf-32"     => Encodings.UTF32,
        _            => Encodings.Default
    };

    public int ToIndex() => (int) ToEncodings(BodyName!);

    //=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    #endregion Methods
}