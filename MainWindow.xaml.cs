using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sluggity
{
    public partial class MainWindow : Window
    {
        private Player MyPlayer;
        private DispatcherTimer timer;
        private const double CameraFollowOffsetX = 480; // Adjust as needed
        private const double CameraFollowOffsetY = 360; // Adjust as needed

        public MainWindow()
        {
            InitializeComponent();

            MyPlayer = new Player(GameCanvas, PlayerRect)
            {
                Position = new Point(50, 850)
            };
            this.DataContext = MyPlayer;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += Timer_Tick;
            timer.Start();

            Focus();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MyPlayer.Move();

            // Calculate camera position
            double cameraX = MyPlayer.Position.X - CameraFollowOffsetX;
            double cameraY = MyPlayer.Position.Y - CameraFollowOffsetY;

            // Ensure camera doesn't go out of bounds

            
            // Adjust canvas position to simulate camera movement
            GameCanvas.RenderTransform = new TranslateTransform(-cameraX, -cameraY);
        }
    }
}
