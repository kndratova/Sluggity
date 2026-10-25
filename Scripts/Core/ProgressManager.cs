using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Sluggity.Core
{
    public class LevelProgress
    {
        public bool IsCompleted { get; set; }
        public int BestPearls { get; set; }
    }

    public static class ProgressManager
    {
        private static readonly string s_filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sluggity", "progress.json");

        private static readonly Dictionary<string, LevelProgress> s_progress = Load();

        public static LevelProgress Get(LevelInfo level)
        {
            return s_progress.TryGetValue(level.Id, out LevelProgress progress) ? progress : new LevelProgress();
        }

        public static bool IsUnlocked(LevelInfo level)
        {
            return level.Number == 1 || Get(SceneManager.Levels[level.Number - 2]).IsCompleted;
        }

        public static void SaveResult(LevelInfo level, int pearls)
        {
            if (!s_progress.TryGetValue(level.Id, out LevelProgress progress))
            {
                progress = new LevelProgress();
                s_progress[level.Id] = progress;
            }

            progress.IsCompleted = true;
            progress.BestPearls = Math.Max(progress.BestPearls, pearls);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(s_filePath) ?? string.Empty);
                File.WriteAllText(s_filePath, JsonSerializer.Serialize(s_progress));
            }
            catch (IOException e)
            {
                Console.WriteLine("Can't save progress: " + e.Message);
            }
        }

        private static Dictionary<string, LevelProgress> Load()
        {
            try
            {
                if (File.Exists(s_filePath))
                {
                    return JsonSerializer.Deserialize<Dictionary<string, LevelProgress>>(File.ReadAllText(s_filePath));
                }
            }
            catch (Exception e) when (e is IOException or JsonException)
            {
                Console.WriteLine("Can't load progress: " + e.Message);
            }

            return new Dictionary<string, LevelProgress>();
        }
    }
}
