using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sluggity.Core.Rendering
{
    public class RectangleView : ObjectView
    {
        private readonly Rectangle m_rectangle;

        public RectangleView(double width, double height, Color? fill = null, Color? stroke = null, double strokeThickness = 1)
        {
            m_rectangle = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = CreateBrush(fill),
                Stroke = CreateBrush(stroke),
                StrokeThickness = strokeThickness,
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

        public double StrokeThickness
        {
            get => m_rectangle.StrokeThickness;
            set => m_rectangle.StrokeThickness = value;
        }

        public void SetFill(Color? color)
        {
            m_rectangle.Fill = CreateBrush(color);
        }

        public void SetStroke(Color? color)
        {
            m_rectangle.Stroke = CreateBrush(color);
        }
        
        private static Brush CreateBrush(Color? color)
        {
            if (color == null)
            {
                return null;
            }

            var brush = new SolidColorBrush(color.Value);
            brush.Freeze();
            return brush;
        }
    }
}