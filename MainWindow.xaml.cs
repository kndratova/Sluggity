using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sluggity
{
    public class Player : INotifyPropertyChanged
    {
        private Point _position;
        public Point Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }

        private double _velocityY;
        public double VelocityY
        {
            get { return _velocityY; }
            set
            {
                if (_velocityY != value)
                {
                    _velocityY = value;
                    OnPropertyChanged(nameof(VelocityY));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MainWindow : Window
    {
        public Player MyPlayer { get; set; }
        private DispatcherTimer timer;
        private float moveDistance = 5;
        private bool isJumping = false;
        private int jumpDuration = 20; // Number of timer ticks for the jump
        private List<Rectangle> obstacles;

        public MainWindow()
        {
            InitializeComponent();

            MyPlayer = new Player
            {
                Position = new Point(10, 100)
            };
            this.DataContext = MyPlayer;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += Timer_Tick;
            timer.Start();

            Focus();

            // Initialize obstacles
            obstacles = new List<Rectangle>();
            AddObstacle(0, 300, 200, 20);
            AddObstacle(200, 150, 200, 20);
            // Add more obstacles as needed
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Apply gravity
            MyPlayer.VelocityY += 0.5; // Increase velocity by gravity

            // Check for keyboard input and update player position
            if (Keyboard.IsKeyDown(Key.Right))
            {
                MyPlayer.Position = new Point(MyPlayer.Position.X + moveDistance, MyPlayer.Position.Y);
            }
            if (Keyboard.IsKeyDown(Key.Left))
            {
                MyPlayer.Position = new Point(MyPlayer.Position.X - moveDistance, MyPlayer.Position.Y);
            }

            // Check for jump action
            if (Keyboard.IsKeyDown(Key.Space) && !isJumping)
            {
                isJumping = true;
                MyPlayer.VelocityY = -15; // Set upward velocity for jump
            }

            // Update player position based on velocity
            MyPlayer.Position = new Point(MyPlayer.Position.X, MyPlayer.Position.Y + MyPlayer.VelocityY);

            // Check for collision with obstacles
            foreach (var obstacle in obstacles)
            {
                if (CheckCollision(MyPlayer, obstacle))
                {
                    MyPlayer.Position = new Point(MyPlayer.Position.X, obstacle.Margin.Top - 50); // Assuming player height is 50
                    MyPlayer.VelocityY = 0; // Stop falling when colliding with the obstacle
                    isJumping = false; // Allow jumping again
                }
            }
        }

        private bool CheckCollision(Player player, Rectangle obstacle)
        {
            // Check if player collides with obstacle
            return player.Position.X + 50 >= obstacle.Margin.Left &&
                   player.Position.X <= obstacle.Margin.Left + obstacle.Width &&
                   player.Position.Y + 50 >= obstacle.Margin.Top &&
                   player.Position.Y <= obstacle.Margin.Top + obstacle.Height;
        }

        private void AddObstacle(double x, double y, double width, double height)
        {
            var obstacle = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(x, y, 0, 0)
            };
            GameCanvas.Children.Add(obstacle);
            obstacles.Add(obstacle);
        }
    }
}
