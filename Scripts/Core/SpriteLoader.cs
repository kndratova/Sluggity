using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Sluggity.Core
{
    public static class SpriteLoader
    {
        private static readonly Dictionary<string, BitmapSource> s_cache = new Dictionary<string, BitmapSource>();

        public static BitmapSource Get(string name)
        {
            if (s_cache.TryGetValue(name, out BitmapSource sprite))
            {
                return sprite;
            }

            sprite = new BitmapImage(new Uri($"pack://application:,,,/Sprites/{name}.png"));
            s_cache[name] = sprite;
            return sprite;
        }

        public static BitmapSource Crop(string name, Int32Rect area)
        {
            string key = $"{name}_{area}";
            if (s_cache.TryGetValue(key, out BitmapSource sprite))
            {
                return sprite;
            }

            sprite = new CroppedBitmap(Get(name), area);
            s_cache[key] = sprite;
            return sprite;
        }
    }
}
