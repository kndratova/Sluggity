using Sluggity.Core.Objects;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Core.Physics
{
    public class PhysicsBody : Component
    {
        public double Gravity { get; set; } = 2400;
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
        public bool IsGrounded { get; private set; }

        // calls by the scene AFTER all scripted movement in this fixed step
        public void Step(Scene scene, double deltaTime)
        {
            VelocityY -= Gravity * deltaTime;

            Owner.X += VelocityX * deltaTime;
            foreach (Box other in OverlappingSolids(scene))
            {
                Box bounds = Owner.Collider.GetBounds();
                Owner.X += VelocityX > 0 ? other.X - bounds.Right : other.Right - bounds.X;
                VelocityX = 0;
            }

            IsGrounded = false;
            Owner.Y += VelocityY * deltaTime;
            foreach (Box other in OverlappingSolids(scene))
            {
                Box bounds = Owner.Collider.GetBounds();
                if (VelocityY <= 0)
                {
                    Owner.Y += other.Top - bounds.Y;
                    IsGrounded = true;
                }
                else
                {
                    Owner.Y += other.Y - bounds.Top;
                }

                VelocityY = 0;
            }
        }

        private System.Collections.Generic.IEnumerable<Box> OverlappingSolids(Scene scene)
        {
            foreach (Collider solid in scene.Collisions.Solids)
            {
                if (solid == Owner.Collider)
                {
                    continue;
                }

                Box other = solid.GetBounds();
                if (Owner.Collider.GetBounds().Overlaps(other))
                {
                    yield return other;
                }
            }
        }
    }
}