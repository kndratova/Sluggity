using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Sluggity.Core;

namespace Sluggity.Pages
{
    public partial class MainMenu : Page
    {
        private const double k_jumpHeight = 60;
        private const double k_jumpTime = 0.25;
        private const double k_walkSpeed = 250;
        private const double k_maxFrameTime = 0.1;

        private readonly GameInput m_input = new GameInput();

        private Window m_window;
        private TimeSpan m_lastFrameTime;
        private bool m_isJumping;

        public MainMenu()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            m_window = Window.GetWindow(this);
            m_window.PreviewKeyDown += OnKeyDown;
            m_window.PreviewKeyUp += OnKeyUp;
            m_window.Deactivated += OnWindowDeactivated;

            m_lastFrameTime = TimeSpan.Zero;
            CompositionTarget.Rendering += OnRendering;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= OnRendering;

            m_window.PreviewKeyDown -= OnKeyDown;
            m_window.PreviewKeyUp -= OnKeyUp;
            m_window.Deactivated -= OnWindowDeactivated;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            TimeSpan frameTime = ((RenderingEventArgs)e).RenderingTime;
            if (frameTime == m_lastFrameTime)
            {
                return;
            }

            double deltaTime = 0;
            if (m_lastFrameTime != TimeSpan.Zero)
            {
                deltaTime = Math.Min((frameTime - m_lastFrameTime).TotalSeconds, k_maxFrameTime);
            }

            m_lastFrameTime = frameTime;

            int direction = 0;
            if (m_input.Right)
            {
                direction++;
            }

            if (m_input.Left)
            {
                direction--;
            }

            if (direction != 0)
            {
                Mike.Source = SpriteLoader.Get(direction > 0 ? "mike_right" : "mike_left");
                MikeTransform.X += direction * k_walkSpeed * deltaTime;
                WrapAroundScreen();
            }

            if (m_input.JumpPressed)
            {
                Jump();
            }

            m_input.EndFrame();
        }

        private void WrapAroundScreen()
        {
            double edge = (ActualWidth + Mike.Width) / 2;
            if (MikeTransform.X > edge)
            {
                MikeTransform.X = -edge;
            }
            else if (MikeTransform.X < -edge)
            {
                MikeTransform.X = edge;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            m_input.Press(e.Key);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            m_input.Release(e.Key);
        }

        private void OnWindowDeactivated(object sender, EventArgs e)
        {
            m_input.Reset();
        }

        private void Jump()
        {
            if (m_isJumping)
            {
                return;
            }

            m_isJumping = true;

            DoubleAnimation animation = new DoubleAnimation(0, -k_jumpHeight, TimeSpan.FromSeconds(k_jumpTime))
            {
                AutoReverse = true,
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut },
            };
            animation.Completed += (_, _) => m_isJumping = false;

            MikeTransform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LevelSelectionPage());
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
