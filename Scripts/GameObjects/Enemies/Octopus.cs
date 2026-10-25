using Sluggity.Core;

namespace Sluggity.GameObjects.Enemies
{
    public class Octopus : Enemy
    {
        private const double k_speed = 110;
        private const double k_probe = 4;

        private bool m_movingUp = true;
        private bool m_wallOnLeft;
        private bool m_wallChecked;

        public Octopus(double x, double y) : base(x, y, 100, 140, SpriteLoader.Get("octopus_up_right"))
        {
        }

        public override void Update(Scene scene, double deltaTime)
        {
            if (!m_wallChecked)
            {
                m_wallOnLeft = scene.IsSolidAt(X - k_probe, CenterY);
                m_wallChecked = true;
            }

            double wallX = m_wallOnLeft ? X - k_probe : Right + k_probe;
            double frontY = m_movingUp ? Top : Y;
            if (!scene.IsSolidAt(wallX, frontY))
            {
                m_movingUp = !m_movingUp;
            }

            VelocityY = m_movingUp ? k_speed : -k_speed;
            Move(scene, deltaTime);

            if (HitFloorOrCeiling)
            {
                m_movingUp = !m_movingUp;
            }

            string vertical = m_movingUp ? "up" : "down";
            string side = m_wallOnLeft ? "right" : "left";
            SetSprite(SpriteLoader.Get($"octopus_{vertical}_{side}"));
        }
    }
}
