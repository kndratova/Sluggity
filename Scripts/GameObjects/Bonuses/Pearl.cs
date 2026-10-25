using System;
using Sluggity.Core;

namespace Sluggity.GameObjects.Bonuses
{
    public class Pearl(double x, double y) : ObjectSprite(x, y, 40, 39, SpriteLoader.Get("pearl"))
    {
        private readonly double m_startY = y;
        private double m_time = x;

        public override void Update(Scene scene, double deltaTime)
        {
            m_time += deltaTime;
            Y = m_startY + Math.Sin(m_time * 3) * 6;
        }

        public override void OnPlayerTouch(Player player, Scene scene)
        {
            Destroy();
            scene.CollectPearl();
        }
    }
}
