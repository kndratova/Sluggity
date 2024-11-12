using System;
using System.Windows.Controls;
using System.Windows.Input;
using Sluggity.GameObjects;
using System.Collections.Generic;

namespace Sluggity.Core
{
    internal sealed class GameCore
    {
        private static GameCore _instance;

        private GameCore() { }

        public static GameCore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameCore();
                }
                return _instance;
            }
        }

        public static Canvas GameCanvas { get; private set; }

        private static bool _spacePressedLastFrame = false;
        private static bool _escapePressedLastFrame = false;
        private static bool _isPaused = false;

        public static event EventHandler Update
        {
            add => TimerManager.Update += value;
            remove => TimerManager.Update -= value;
        }

        public void Construct(Canvas gameCanvas)
        {
            InitializeGame(gameCanvas);
            Update += OnUpdate;
        }

        private void InitializeGame(Canvas gameCanvas)
        {
            TimerManager.StartTimer();
            GameCanvas = gameCanvas;
            GameCanvas.Focus();
            SceneManager.Construct(GameCanvas);
        }

        private static void OnUpdate(object sender, EventArgs e)
        {
            HandleInput();
            if (!_isPaused)
            {
                UpdateGameObjects();
            }
        }

        private static void UpdateGameObjects()
        {
            var _currentSceneGO = new List<GameObject>(SceneManager.SceneGameObjects);
            foreach (var gameObject in _currentSceneGO)
            {
                gameObject.Update();
            }
        }

        private static void HandleInput()
        {

            if (Keyboard.IsKeyDown(Key.Escape) && !_escapePressedLastFrame)
            {
                TogglePause();
            }
            _escapePressedLastFrame = Keyboard.IsKeyDown(Key.Escape);
        }

        private static void TogglePause()
        {
            _isPaused = !_isPaused;
            if (_isPaused)
            {
                Console.WriteLine("Game is paused.");
            } else
            {
                Console.WriteLine("Game is resumed.");
            }
        }
    }
}
