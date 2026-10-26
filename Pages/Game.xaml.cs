using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Sluggity.Core;
using Sluggity.Core.Input;
using Sluggity.Core.Objects;
using Sluggity.Core.Rendering;
using Sluggity.Core.SceneManagement;
using Sluggity.Gameplay.Player;

namespace Sluggity.Pages;

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
        
        var go = m_scene.Add(new GameObject(50, 50));
        go.AddComponent(new SpriteView("crub_left"));
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
        m_scene.Start();
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
        m_loop.Tick += OnTick;
        m_loop.Start();
    }

    private void StopLoop()
    {
        m_loop.Stop();
        m_loop.Tick -= OnTick;
    }

    private void OnTick(double deltaTime)
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