using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sluggity.GameObjects
{
    public class Obstacle : GameObject
    {
        private static readonly Brush s_fillBrush = new SolidColorBrush(Color.FromRgb(0, 2, 44));

        private readonly Rectangle m_rectangle;

        public Obstacle(double x, double y, double width, double height) : base(x, y, width, height)
        {
            m_rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = s_fillBrush,
            };
        }

        public double DeltaX { get; protected set; }
        public double DeltaY { get; protected set; }

        public override FrameworkElement Visual => m_rectangle;
    }
}
