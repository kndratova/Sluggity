using System.Windows.Media.Imaging;
using Sluggity.Core;
using Sluggity.GameObjects.Bonuses;

namespace Sluggity.ViewModels
{
    public class GameHudViewModel(LevelInfo level) : ViewModelBase
    {
        private int m_pearlsCollected;
        private bool m_hasKnife;
        private bool m_hasLegs;
        private bool m_isPaused;
        private bool m_isCompleted;
        private string m_resultTitle;

        public string LevelTitle { get; } = $"{level.Number}. {level.Name}";
        public int PearlsTotal { get; } = level.PearlCount;
        public bool HasNextLevel { get; } = SceneManager.GetNext(level) != null;

        public BitmapSource PearlIcon => SpriteLoader.Get("pearl");
        public BitmapSource KnifeIcon => PowerUp.GetIcon(PowerUpType.Knife);
        public BitmapSource LegsIcon => PowerUp.GetIcon(PowerUpType.Legs);

        public int PearlsCollected
        {
            get => m_pearlsCollected;
            private set => Set(ref m_pearlsCollected, value);
        }

        public bool HasKnife
        {
            get => m_hasKnife;
            private set => Set(ref m_hasKnife, value);
        }

        public bool HasLegs
        {
            get => m_hasLegs;
            private set => Set(ref m_hasLegs, value);
        }

        public bool IsPaused
        {
            get => m_isPaused;
            set => Set(ref m_isPaused, value);
        }

        public bool IsCompleted
        {
            get => m_isCompleted;
            set => Set(ref m_isCompleted, value);
        }

        public string ResultTitle
        {
            get => m_resultTitle;
            set => Set(ref m_resultTitle, value);
        }

        public void Update(Scene scene)
        {
            PearlsCollected = scene.PearlsCollected;
            HasKnife = scene.Player.HasKnife;
            HasLegs = scene.Player.HasLegs;
        }
    }
}
