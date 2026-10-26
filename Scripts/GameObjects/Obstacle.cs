using System.Windows.Media;
using Sluggity.Core.GameObject;

namespace Sluggity.GameObjects
{
    public class Obstacle : GameObject
    {
        public Obstacle(double x, double y, double width, double height, Color color) : base(x, y, width, height)
        {
            View = new RectangleView(width, height, color);
        }

        public double DeltaX { get; protected set; }
        public double DeltaY { get; protected set; }
    }
}