using System.Windows;
using System.Windows.Controls;

namespace Sluggity.Pages
{
    public partial class LevelSelectionPage : Page
    {
        private int[] levelStars = { 0, 0, 0 };
        private const int MaxStarsPerLevel = 3;

        public LevelSelectionPage()
        {
            InitializeComponent();
            UpdateLevelStatus();
        }

        private void UpdateLevelStatus()
        {
            Level1Stars.Text = $"{levelStars[0]}/3";
            if (levelStars[0] > 0) Level2Button.IsEnabled = true;

            Level2Stars.Text = $"{levelStars[1]}/3";
            if (levelStars[1] > 0) Level3Button.IsEnabled = true;

            Level3Stars.Text = $"{levelStars[2]}/3";
        }

        private void Level1Button_Click(object sender, RoutedEventArgs e)
        {
            levelStars[0] = MaxStarsPerLevel;
            NavigationService?.Navigate(new Level1());
            UpdateLevelStatus();
        }

        private void Level2Button_Click(object sender, RoutedEventArgs e)
        {
            levelStars[1] = MaxStarsPerLevel;
            UpdateLevelStatus();
        }

        private void Level3Button_Click(object sender, RoutedEventArgs e)
        {
            levelStars[2] = MaxStarsPerLevel;
            UpdateLevelStatus();
        }
    }
}
