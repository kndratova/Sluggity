using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Sluggity.Core;
using Sluggity.GameObjects.Bonuses;
using Sluggity.ViewModels;

namespace Sluggity.Pages
{
    public partial class GamePage : Page
    {
        private const double k_viewWidth = 960;
        private const double k_viewHeight = 600;
        private const double k_maxFrameTime = 1.0 / 30;
        private const double k_backgroundParallax = 0.15;

        private readonly LevelInfo m_level;
        private readonly GameInput m_input = new GameInput();
        private readonly Camera m_camera = new Camera(k_viewWidth, k_viewHeight);
        private readonly GameHudViewModel m_hud;

        private Scene m_scene;
        private Window m_window;
        private TimeSpan m_lastFrameTime;

        public GamePage(LevelInfo level)
        {
            InitializeComponent();

            m_level = level;
            m_hud = new GameHudViewModel(level);
            DataContext = m_hud;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            StartLevel();
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

        private void StartLevel()
        {
            m_scene = new Scene(m_level.Data, GameCanvas, m_input);
            m_scene.PowerUpCollected += OnPowerUpCollected;

            m_hud.IsPaused = false;
            m_hud.IsCompleted = false;
            m_hud.Update(m_scene);

            m_camera.SnapTo(m_scene.Player, m_scene);
            UpdateCamera();
        }

        private void OnRendering(object sender, EventArgs e)
        {
            TimeSpan frameTime = ((RenderingEventArgs)e).RenderingTime;
            if (frameTime == m_lastFrameTime)
            {
                return;
            }

            double deltaTime = 0.0;
            if (m_lastFrameTime != TimeSpan.Zero)
            {
                deltaTime = Math.Min((frameTime - m_lastFrameTime).TotalSeconds, k_maxFrameTime);
            }

            m_lastFrameTime = frameTime;

            if (m_hud.IsPaused || m_hud.IsCompleted)
            {
                return;
            }

            m_scene.Update(deltaTime);
            m_input.EndFrame();
            m_hud.Update(m_scene);

            if (m_scene.State == GameState.Died)
            {
                ((Storyboard)FindResource("DeathAnimation")).Begin(this);
                StartLevel();
                return;
            }

            if (m_scene.State == GameState.Completed)
            {
                CompleteLevel();
            }

            m_camera.Follow(m_scene.Player, m_scene, deltaTime);
            UpdateCamera();
        }

        private void UpdateCamera()
        {
            CameraTransform.X = -m_camera.X;
            CameraTransform.Y = k_viewHeight + m_camera.Y;

            Canvas.SetLeft(BackgroundImage, -m_camera.X * k_backgroundParallax);
            Canvas.SetTop(BackgroundImage, k_viewHeight - BackgroundImage.Height + m_camera.Y * k_backgroundParallax);
        }

        private void CompleteLevel()
        {
            ProgressManager.SaveResult(m_level, m_scene.PearlsCollected);
            m_hud.ResultTitle = m_hud.HasNextLevel ? "Level complete!" : "You beat the game!";
            m_hud.IsCompleted = true;
        }

        private void TogglePause()
        {
            m_hud.IsPaused = !m_hud.IsPaused;
            m_input.EndFrame();
        }

        private void OnPowerUpCollected(PowerUpType type)
        {
            ToastText.Text = type == PowerUpType.Knife
                ? "Got a knife! Face enemies to slice them"
                : "Got legs! Jump again in the air";
            ((Storyboard)FindResource("ToastAnimation")).Begin(this);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System)
            {
                return;
            }

            if (e.Key == Key.Escape && !m_hud.IsCompleted)
            {
                TogglePause();
            }
            else if (e.Key == Key.R)
            {
                StartLevel();
            }
            else
            {
                m_input.Press(e.Key);
            }

            e.Handled = true;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            m_input.Release(e.Key);
        }

        private void OnWindowDeactivated(object sender, EventArgs e)
        {
            m_input.Reset();
            if (!m_hud.IsCompleted)
            {
                m_hud.IsPaused = true;
            }
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            TogglePause();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            StartLevel();
        }

        private void LevelsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LevelSelectionPage());
        }

        private void NextLevelButton_Click(object sender, RoutedEventArgs e)
        {
            LevelInfo next = SceneManager.GetNext(m_level);
            if (next != null)
            {
                NavigationService?.Navigate(new GamePage(next));
            }
        }
    }
}
