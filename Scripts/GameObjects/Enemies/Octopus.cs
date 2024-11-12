using System;
using System.Windows.Media.Imaging;

namespace Sluggity.GameObjects.Enemies
{
    public class Octopus : Enemy
    {
        private readonly BitmapImage _octopusSpriteUp = new(new Uri("pack://application:,,,/Sprites/octopus_up_right.png"));
        private readonly BitmapImage _octopusSpriteDown = new(new Uri("pack://application:,,,/Sprites/octopus_down_right.png"));

        public Octopus((float, float) position) : base(position)
        {
            _enemySpriteLeft = _octopusSpriteUp;
            _enemySpriteRight = _octopusSpriteDown;
            EnemyMoveSpeed = 2;
            _enemyMoveDirectionVector = DirectionVector.Up;
            _enemySprite = new ObjectSprite((X, Y), this)
            {
                ObjectImage = GetBitmapImage()
            };
        }

        protected override BitmapImage GetBitmapImage()
        {
            return _enemyMoveDirectionVector == DirectionVector.Up ? _enemySpriteLeft : _enemySpriteRight;
        }

        protected override void Walk()
        {
            _velocity.Item2 = _enemyMoveDirectionVector == DirectionVector.Up ? EnemyMoveSpeed : -EnemyMoveSpeed;
        }
    }
}