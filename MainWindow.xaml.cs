using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sluggity
{
    public partial class MainWindow : Window
    {
        public Player MyPlayer { get; set; }
        private DispatcherTimer timer;
        

        public MainWindow()
        {
            InitializeComponent();

            MyPlayer = new Player(GameCanvas, PlayerRect)
            {
                Position = new Point(0, 0)
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
        }

        
    }
}
