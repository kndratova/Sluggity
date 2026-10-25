using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sluggity.Core
{
    public static class SceneManager
    {
        private const string k_scenesDirectory = "Scenes";

        private static List<LevelInfo> s_levels;

        public static List<LevelInfo> Levels => s_levels ??= LoadLevels();

        public static LevelInfo GetNext(LevelInfo level)
        {
            return level.Number < Levels.Count ? Levels[level.Number] : null;
        }

        private static List<LevelInfo> LoadLevels()
        {
            List<string> files = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, k_scenesDirectory), "*.json")
                .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                .ToList();

            List<LevelInfo> levels = new List<LevelInfo>();
            for (int i = 0; i < files.Count; i++)
            {
                levels.Add(new LevelInfo(Path.GetFileNameWithoutExtension(files[i]), i + 1, SceneData.Load(files[i])));
            }

            return levels;
        }
    }
}
