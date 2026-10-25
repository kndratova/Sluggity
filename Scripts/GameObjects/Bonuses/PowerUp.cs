using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Sluggity.Core;

namespace Sluggity.GameObjects.Bonuses
{
    public enum PowerUpType
    {
        Knife,
        Legs,
    }

    public class PowerUp : ObjectSprite
    {
        private readonly double m_startY;
        private double m_time;

        public PowerUp(double x, double y, PowerUpType type) : base(x, y, 70, 60, GetIcon(type))
        {
            Type = type;
            m_startY = y;
        }

        public PowerUpType Type { get; }

        public static BitmapSource GetIcon(PowerUpType type)
        {
            return type == PowerUpType.Knife
                ? SpriteLoader.Crop("mike_knife_right", new Int32Rect(138, 0, 71, 52))
                : SpriteLoader.Crop("mike_legs_right", new Int32Rect(28, 48, 80, 60));
        }

        public override void Update(Scene scene, double deltaTime)
        {
            m_time += deltaTime;
            Y = m_startY + Math.Sin(m_time * 2.5) * 8;
        }

        public override void OnPlayerTouch(Player player, Scene scene)
        {
            Destroy();
            scene.CollectPowerUp(Type);
        }
    }
}
