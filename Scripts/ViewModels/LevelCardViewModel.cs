using Sluggity.Core;

namespace Sluggity.ViewModels
{
    public class LevelCardViewModel
    {
        public LevelCardViewModel(LevelInfo level)
        {
            Level = level;
            IsUnlocked = ProgressManager.IsUnlocked(level);

            LevelProgress progress = ProgressManager.Get(level);
            IsCompleted = progress.IsCompleted;
            PearlsText = $"{progress.BestPearls} / {level.PearlCount}";
        }

        public LevelInfo Level { get; }
        public int Number => Level.Number;
        public string Name => Level.Name;
        public bool IsUnlocked { get; }
        public bool IsCompleted { get; }
        public string PearlsText { get; }
    }
}
