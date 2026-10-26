using System.Windows;
using System.Windows.Controls;
using Sluggity.Core.Objects;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Core.Rendering
{
    public abstract class ObjectView : Component
    {
        public abstract FrameworkElement Element { get; }

        // fixed shift in world units
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }

        // normalized pivot
        public double PivotX { get; set; }
        public double PivotY { get; set; }

        public override void OnAddedToScene(Scene scene)
        {
            scene.Canvas.Children.Add(Element);
            Draw(scene.Camera);
        }

        public override void OnRemovedFromScene(Scene scene)
        {
            scene.Canvas.Children.Remove(Element);
        }

        public virtual void Draw(Camera camera)
        {
            double left = Owner.RenderX + OffsetX - Element.Width * PivotX;
            double bottom = Owner.RenderY + OffsetY - Element.Height * PivotY;

            Point screen = camera.WorldToScreen(left, bottom + Element.Height);
            Canvas.SetLeft(Element, screen.X);
            Canvas.SetTop(Element, screen.Y);
        }
    }
}