using System;
using Sluggity.Core.Objects;
using Sluggity.Core.Rendering;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Gameplay
{
    public class CameraRig : GameObject
    {
        private CameraBounds? m_bounds;

        public CameraRig(GameObject target, CameraBounds? cameraBounds = null) : base(0, 0)
        {
            Target = target;
            m_bounds = cameraBounds;
        }

        public GameObject Target { get; set; }
        public double Smoothing { get; set; } = 8;
        public double AnchorX { get; set; } = 0.5;
        public double AnchorY { get; set; } = 0.35;

        protected override void OnStart(Scene scene)
        {
            Move(scene.Camera, 1);
        }

        protected override void OnLateUpdate(Scene scene, double deltaTime)
        {
            double t = Smoothing > 0 ? 1 - Math.Exp(-Smoothing * deltaTime) : 1;
            Move(scene.Camera, t);
        }

        private void Move(Camera camera, double t)
        {
            if (Target == null || Target.IsDestroyed)
            {
                return;
            }

            double targetX = Target.RenderX - camera.ViewWidth * AnchorX;
            double targetY = Target.RenderY - camera.ViewHeight * AnchorY;
            
            targetX = camera.X + (targetX - camera.X) * t;
            targetY = camera.Y + (targetY - camera.Y) * t;

            
            if (m_bounds is { } bounds)
            {
                targetX = Math.Clamp(targetX, bounds.MinX, bounds.MaxX - camera.ViewWidth);
                targetY = Math.Clamp(targetY, bounds.MinY, bounds.MaxY - camera.ViewHeight);
            }
            
            camera.X = targetX;
            camera.Y = targetY;
        }
    }

    public readonly struct CameraBounds
    {
        private readonly double m_minX;
        private readonly double m_maxX;
        private readonly double m_minY;
        private readonly double m_maxY;

        public double MinX => m_minX;
        public double MaxX => m_maxX;
        public double MinY => m_minY;
        public double MaxY => m_maxY;

        public CameraBounds(double minX, double maxX, double minY, double maxY)
        {
            if (minX > maxX)
            {
                throw new ArgumentException($"minX ({minX}) is greater than maxX ({maxX})");
            }

            if (minY > maxY)
            {
                throw new ArgumentException($"minY ({minY}) is greater than maxY ({maxY})");
            }

            m_minX = minX;
            m_maxX = maxX;
            m_minY = minY;
            m_maxY = maxY;
        }
    }
}