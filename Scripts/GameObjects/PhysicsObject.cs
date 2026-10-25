using System;
using System.Windows.Media.Imaging;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public abstract class PhysicsObject : ObjectSprite
    {
        protected const double Gravity = 2400;
        private const double k_maxFallSpeed = 1100;

        protected PhysicsObject(double x, double y, double width, double height, BitmapSource sprite)
            : base(x, y, width, height, sprite)
        {
        }

        public double VelocityX { get; protected set; }
        public double VelocityY { get; protected set; }
        public bool IsGrounded => Ground != null;

        protected Obstacle Ground { get; private set; }
        protected bool HitWall { get; private set; }
        protected bool HitFloorOrCeiling { get; private set; }

        protected void ApplyGravity(double deltaTime)
        {
            VelocityY = Math.Max(VelocityY - Gravity * deltaTime, -k_maxFallSpeed);
        }

        protected void Move(Scene scene, double deltaTime)
        {
            if (Ground != null)
            {
                X += Ground.DeltaX;
                Y += Ground.DeltaY;
            }

            double dx = VelocityX * deltaTime;
            double dy = VelocityY * deltaTime;

            HitWall = false;
            X += dx;
            foreach (Obstacle obstacle in scene.Obstacles)
            {
                if (!Overlaps(obstacle))
                {
                    continue;
                }

                double relativeDx = dx - obstacle.DeltaX;
                if (relativeDx > 0)
                {
                    X = obstacle.X - Width;
                }
                else if (relativeDx < 0)
                {
                    X = obstacle.Right;
                }

                HitWall = true;
            }

            Ground = null;
            HitFloorOrCeiling = false;
            Y += dy;
            foreach (Obstacle obstacle in scene.Obstacles)
            {
                if (!Overlaps(obstacle))
                {
                    continue;
                }

                if (VelocityY <= 0)
                {
                    Y = obstacle.Top;
                    Ground = obstacle;
                }
                else
                {
                    Y = obstacle.Y - Height;
                }

                VelocityY = 0;
                HitFloorOrCeiling = true;
            }
        }
    }
}
