using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sluggity.Core
{
    internal static class Game
    {
        private static bool _spacePressedLastFrame = false;

        public static Canvas GameCanvas { get; private set; }

        public static event EventHandler Update
        {
            add => TimerManager.Update += value;
            remove => TimerManager.Update -= value;
        }

        public static void Construct(Canvas gameCanvas)
        {
            InitializeGame(gameCanvas);
            Update += OnUpdate;
        }

        private static void InitializeGame(Canvas gameCanvas)
        {
            TimerManager.StartTimer();
            GameCanvas = gameCanvas;
            GameCanvas.Focus();
            SceneManager.Construct(gameCanvas);
        }

        private static void OnUpdate(object sender, EventArgs e)
        {
            UpdateGameObjects();
            HandleInput();
        }

        private static void UpdateGameObjects()
        {
            foreach (var gameObject in SceneManager.SceneGameObjects)
            {
                gameObject.Update();
            }
        }

        private static void HandleInput()
        {
            var spacePressedThisFrame = Keyboard.IsKeyDown(Key.Space);

            if (spacePressedThisFrame && !_spacePressedLastFrame)
            {
                SceneManager.ReloadCurrentScene();
            }

            _spacePressedLastFrame = spacePressedThisFrame;
        }
    }
}