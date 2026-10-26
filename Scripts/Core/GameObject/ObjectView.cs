using System.Windows;
using System.Windows.Controls;

namespace Sluggity.Core.GameObject;

public abstract class ObjectView
{
    public abstract FrameworkElement Element { get; }

    public virtual void Draw(GameObjects.GameObject owner, Camera camera)
    {
        Point screen = camera.WorldToScreen(owner.Bounds.X, owner.Bounds.Top);
        Canvas.SetLeft(Element, screen.X);
        Canvas.SetTop(Element, screen.Y);
    }
}