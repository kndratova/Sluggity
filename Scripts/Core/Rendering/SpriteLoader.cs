using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Sluggity.Core.Rendering;

public static class SpriteLoader
{
    private static readonly Dictionary<string, BitmapSource> s_cache = new Dictionary<string, BitmapSource>();

    public static BitmapSource Get(string name)
    {
        if (s_cache.TryGetValue(name, out BitmapSource sprite))
        {
            return sprite;
        }

        sprite = Load(name);
        s_cache[name] = sprite;
        return sprite;
    }

    private static BitmapSource Load(string name)
    {
        Uri uri = new Uri($"pack://application:,,,/Sprites/{name}.png", UriKind.Absolute);

        try
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException($"Sprite '{name}' not found at {uri}", exception);
        }
    }
}