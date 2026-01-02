// ****************************************************************************
// Project:  AutoUpdater.NET
// File:     Image.cs
// Author:   Latency McLaughlin
// Date:     09/29/2025
// ****************************************************************************

using System.IO;
using System.Net.Cache;
using System.Windows.Media.Imaging;

namespace AutoUpdaterDotNET.Extensions;

public static class ImageExtensions
{
    public static BitmapImage? ConvertToBitmapImage(this Uri resourceUri) => CreateNewBitmapImage(image => image.UriSource = resourceUri);


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
}