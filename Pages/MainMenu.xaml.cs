using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Sluggity.Pages
{
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void PlayButtonMainMenu_Click(object sender, RoutedEventArgs e)
        {
           NavigationService?.Navigate(new LevelSelectionPage());
        }

        private void ExitButtonMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
