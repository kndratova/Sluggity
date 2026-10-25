using System.Windows;
using System.Windows.Controls;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public abstract class GameObject
    {
        protected GameObject(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; protected set; }
        public double Height { get; protected set; }

        public double Right => X + Width;
        public double Top => Y + Height;
        public double CenterX => X + Width / 2;
        public double CenterY => Y + Height / 2;

        public bool IsDestroyed { get; private set; }

        public abstract FrameworkElement Visual { get; }

        public virtual void Update(Scene scene, double deltaTime)
        {
        }

        public virtual void OnPlayerTouch(Player player, Scene scene)
        {
        }

        public virtual void Draw()
        {
            Canvas.SetLeft(Visual, X);
            Canvas.SetTop(Visual, -Top);
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }

        public bool Overlaps(GameObject other)
        {
            return X < other.Right && Right > other.X && Y < other.Top && Top > other.Y;
        }
    }
}
