using System;
using Sluggity.Core.Objects;
using Sluggity.Core.Rendering;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Gameplay
{
    public class CameraRig : GameObject
    {
        public CameraRig(GameObject target) : base(0, 0)
        {
            Target = target;
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

            double targetX = Target.X - camera.ViewWidth * AnchorX;
            double targetY = Target.Y - camera.ViewHeight * AnchorY;

            camera.X = camera.X + (targetX - camera.X) * t;
            camera.Y = camera.Y + (targetY - camera.Y) * t;
        }
    }
}