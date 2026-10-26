using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Sluggity.Core;
using Sluggity.Core.Input;
using Sluggity.Core.Objects;
using Sluggity.Core.Physics;
using Sluggity.Core.Rendering;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Pages
{
    public partial class Game : Page
    {
        private readonly Camera m_camera = new Camera();
        private readonly GameLoop m_loop = new GameLoop();
        private readonly GameInput m_gameInput = new GameInput();

        private Scene m_scene;
        private Window m_window;

        public Game()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CreateScene();
            SubscribeToInput();
            StartLoop();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            StopLoop();
            UnsubscribeFromInput();
        }

        private void CreateScene()
        {
            m_camera.ViewHeight = GameCanvas.ActualHeight;
            m_scene = new Scene(GameCanvas, m_camera, m_gameInput);

            CreateTestObjects();

            m_scene.Start();
        }

        private void CreateTestObjects()
        {
            var crab = m_scene.Add(new GameObject(400, 300));
            crab.AddComponent(new SpriteView("crub_left"));
            crab.AddComponent(new Collider(-60, 0, 120, 70, true));
            crab.AddComponent(new PhysicsBody());

            var ground = m_scene.Add(new GameObject(0, 0));
            ground.AddComponent(new RectangleView(800, 50, Colors.DarkGreen));
            ground.AddComponent(new Collider(0, 0, 800, 50, true));
        }

        private void SubscribeToInput()
        {
            m_window = Window.GetWindow(this);
            m_window.KeyDown += OnKeyDown;
            m_window.KeyUp += OnKeyUp;
        }

        private void UnsubscribeFromInput()
        {
            m_window.KeyDown -= OnKeyDown;
            m_window.KeyUp -= OnKeyUp;
        }

        private void StartLoop()
        {
            m_loop.Update += OnUpdate;
            m_loop.FixedUpdate += m_scene.FixedUpdate;
            m_loop.Render += m_scene.Draw;
            m_loop.Start();
        }

        private void StopLoop()
        {
            m_loop.Stop();
            m_loop.Update -= OnUpdate;
            m_loop.FixedUpdate -= m_scene.FixedUpdate;
            m_loop.Render -= m_scene.Draw;
        }

        private void OnUpdate(double deltaTime)
        {
            m_scene.Update(deltaTime);
            m_gameInput.EndFrame();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            m_gameInput.Press(e.Key);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            m_gameInput.Release(e.Key);
        }
    }
}