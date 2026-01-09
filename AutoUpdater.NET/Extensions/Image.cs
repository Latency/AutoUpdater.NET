// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Image.cs
// Author:   Latency McLaughlin
// Date:     09/29/2025
// ****************************************************************************

using System.IO;
using System.Net.Cache;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutoUpdaterDotNET.Extensions;

public static class ImageExtensions
{
    public static BitmapImage ConvertToBitmapImage(this Uri resourceUri) => CreateNewBitmapImage(image => image.UriSource = resourceUri);


    public static BitmapImage? ConvertStreamToBitmapImage(this Stream imageStream, Uri? resourceUri = null)
    {
        return imageStream is not { CanRead: true } ? null : // Or throw an exception
                   CreateNewBitmapImage(OnCreate);

        void OnCreate(BitmapImage image)
        {
            image.StreamSource = imageStream;
            if (resourceUri is not null)
                image.UriSource = resourceUri;
        }
    }


    private static BitmapImage CreateNewBitmapImage(Action<BitmapImage> callback)
    {
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption    = BitmapCacheOption.None;
        image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
        image.CacheOption    = BitmapCacheOption.OnLoad;
        image.CreateOptions  = BitmapCreateOptions.IgnoreImageCache;
        callback.Invoke(image);
        image.EndInit();
        image.Freeze(); // Optional, but good practice for immutability

        return image;
    }

    public static ImageSource? GetFileIcon(string filePath)
    {
        // Check if the file path is valid
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            return null; // or return a default icon

        try
        {
            // Use the System.Drawing.Icon method to extract
            var sysicon = System.Drawing.Icon.ExtractAssociatedIcon(filePath);

            // Convert the System.Drawing.Icon to a WPF ImageSource (BitmapSource)
            var bmpSrc = Imaging.CreateBitmapSourceFromHIcon(
                sysicon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            // Dispose of the native icon handle to prevent memory leaks
            sysicon.Dispose();

            return bmpSrc;
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., file access denied)
            Console.WriteLine($"Error extracting icon: {ex.Message}");
            return null;
        }
    }
}