using Sluggity.Core;

namespace Sluggity.GameObjects.Enemies
{
    public class Crab(double x, double y) : Enemy(x, y, 120, 70, SpriteLoader.Get("crab_right"))
    {
        private const double k_speed = 90;
        private const double k_probe = 4;

        private bool m_movingRight = true;

        public override void Update(Scene scene, double deltaTime)
        {
            double frontX = m_movingRight ? Right + k_probe : X - k_probe;
            if (IsGrounded && !scene.IsSolidAt(frontX, Y - k_probe))
            {
                m_movingRight = !m_movingRight;
            }

            VelocityX = m_movingRight ? k_speed : -k_speed;
            ApplyGravity(deltaTime);
            Move(scene, deltaTime);

            if (HitWall)
            {
                m_movingRight = !m_movingRight;
            }

            SetSprite(SpriteLoader.Get(m_movingRight ? "crab_right" : "crab_left"));
        }
    }
}
