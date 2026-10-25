using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public class Spikes : GameObject
    {
        private const double k_spikeWidth = 27;
        private const double k_spikeHeight = 36;

        private readonly Rectangle m_rectangle;

        public Spikes(double x, double y, double width) : base(x, y, width, k_spikeHeight - 14)
        {
            int count = Math.Max(1, (int)Math.Round(width / k_spikeWidth));

            m_rectangle = new Rectangle
            {
                Width = width,
                Height = k_spikeHeight,
                Fill = new ImageBrush(SpriteLoader.Get("spike"))
                {
                    TileMode = TileMode.Tile,
                    Viewport = new Rect(0, 0, width / count, k_spikeHeight),
                    ViewportUnits = BrushMappingMode.Absolute,
                },
            };
            RenderOptions.SetBitmapScalingMode(m_rectangle, BitmapScalingMode.HighQuality);
        }

        public override FrameworkElement Visual => m_rectangle;

        public override void Draw()
        {
            Canvas.SetLeft(m_rectangle, X);
            Canvas.SetTop(m_rectangle, -Y - k_spikeHeight);
        }

        public override void OnPlayerTouch(Player player, Scene scene)
        {
            scene.KillPlayer();
        }
    }
}
