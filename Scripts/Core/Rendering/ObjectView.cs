using System.Windows;
using System.Windows.Controls;
using Sluggity.Core.Objects;

namespace Sluggity.Core.Rendering;

public abstract class ObjectView
{
    public abstract FrameworkElement Element { get; }

    public virtual void Draw(GameObject owner, Camera camera)
    {
        Point screen = camera.WorldToScreen(owner.Bounds.X, owner.Bounds.Top);
        Canvas.SetLeft(Element, screen.X);
        Canvas.SetTop(Element, screen.Y);
    }
}