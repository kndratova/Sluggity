using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sluggity.Core;
using Sluggity.GameObjects;

namespace Sluggity.Pages;

public partial class Game : Page
{
    private readonly Camera m_camera = new Camera();
    private Obstacle m_obstacle;

    public Game()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        m_camera.ViewHeight = GameCanvas.ActualHeight;

        m_obstacle = new Obstacle(0, 0, 800, 600, Colors.Aqua);
        GameCanvas.Children.Add(m_obstacle.View.Element);
        m_obstacle.Draw(m_camera);
    }
}