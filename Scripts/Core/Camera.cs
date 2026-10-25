using System;
using Sluggity.GameObjects;

namespace Sluggity.Core
{
    public class Camera
    {
        private const double k_followSpeed = 8;

        public Camera(double viewWidth, double viewHeight)
        {
            ViewWidth = viewWidth;
            ViewHeight = viewHeight;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double ViewWidth { get; }
        public double ViewHeight { get; }

        public void Follow(GameObject target, Scene scene, double deltaTime)
        {
            MoveTo(target, scene, 1 - Math.Exp(-k_followSpeed * deltaTime));
        }

        public void SnapTo(GameObject target, Scene scene)
        {
            MoveTo(target, scene, 1);
        }

        private void MoveTo(GameObject target, Scene scene, double amount)
        {
            double targetX = Math.Clamp(target.CenterX - ViewWidth / 2, 0, Math.Max(0, scene.Width - ViewWidth));
            double targetY = Math.Clamp(target.CenterY - ViewHeight * 0.4, 0, Math.Max(0, scene.Height - ViewHeight));

            X += (targetX - X) * amount;
            Y += (targetY - Y) * amount;
        }
    }
}
