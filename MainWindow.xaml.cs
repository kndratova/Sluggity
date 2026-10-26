using System.Windows;
using System.Windows.Navigation;
using Sluggity.Pages;

namespace Sluggity
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // clear navigation history
            MainFrame.Navigated += OnMainFrameNavigated;
            // set size
            Loaded += OnLoaded;

            MainFrame.Navigate(new Game());
        }

        private void OnMainFrameNavigated(object sender, NavigationEventArgs e)
        {
            while (MainFrame.CanGoBack)
            {
                MainFrame.RemoveBackEntry();
            }
        }
        
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            MinWidth = ActualWidth;
            MinHeight = ActualHeight;
            
            SizeToContent = SizeToContent.Manual;
            RootViewbox.Width = double.NaN;
            RootViewbox.Height = double.NaN;
        }
    }
}
