using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Sluggity.Core;
using Sluggity.ViewModels;

namespace Sluggity.Pages
{
    public partial class LevelSelectionPage : Page
    {
        public LevelSelectionPage()
        {
            InitializeComponent();
            LevelList.ItemsSource = SceneManager.Levels.Select(l => new LevelCardViewModel(l)).ToList();
        }

        private void LevelCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement { DataContext: LevelCardViewModel card })
            {
                NavigationService?.Navigate(new GamePage(card.Level));
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainMenu());
        }
    }
}
