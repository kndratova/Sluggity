using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Sluggity.Core.GameObject;

namespace Sluggity.GameObjects
{
    public class RectangleView : ObjectView
    {
        private readonly Rectangle m_rectangle;

        public RectangleView(double width, double height, Color color)
        {
            SolidColorBrush fill = new SolidColorBrush(color);
            fill.Freeze();

            m_rectangle = new Rectangle { Width = width, Height = height, Fill = fill };
        }

        public override FrameworkElement Element => m_rectangle;
    }
}