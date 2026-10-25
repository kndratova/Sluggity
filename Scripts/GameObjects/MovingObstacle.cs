using System;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public class MovingObstacle : Obstacle
    {
        private const double k_speed = 1.2;

        private readonly double m_startX;
        private readonly double m_startY;
        private readonly double m_moveX;
        private readonly double m_moveY;
        private double m_time;

        public MovingObstacle(double x, double y, double width, double height, double moveX, double moveY)
            : base(x, y, width, height)
        {
            m_startX = x;
            m_startY = y;
            m_moveX = moveX;
            m_moveY = moveY;
        }

        public override void Update(Scene scene, double deltaTime)
        {
            m_time += deltaTime;
            double progress = (1 - Math.Cos(m_time * k_speed)) / 2;

            double newX = m_startX + m_moveX * progress;
            double newY = m_startY + m_moveY * progress;
            DeltaX = newX - X;
            DeltaY = newY - Y;
            X = newX;
            Y = newY;
        }
    }
}
