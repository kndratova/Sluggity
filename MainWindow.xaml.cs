using System.Windows;
using Sluggity.Pages;

namespace Sluggity
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigated += (_, _) =>
            {
                while (MainFrame.CanGoBack)
                {
                    MainFrame.RemoveBackEntry();
                }
            };

            MainFrame.Navigate(new MainMenu());
        }
    }
}
