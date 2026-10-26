using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sluggity.Core.Rendering
{
    public class RectangleView : ObjectView
    {
        private readonly Rectangle m_rectangle;

        public RectangleView(double width, double height, Color color)
        {
            m_rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = CreateBrush(color),
            };
        }

        public override FrameworkElement Element => m_rectangle;

        public double Width
        {
            get => m_rectangle.Width;
            set => m_rectangle.Width = value;
        }

        public double Height
        {
            get => m_rectangle.Height;
            set => m_rectangle.Height = value;
        }

        public void SetColor(Color color)
        {
            m_rectangle.Fill = CreateBrush(color);
        }

        private static Brush CreateBrush(Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }
    }
}