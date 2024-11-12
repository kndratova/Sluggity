using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Sluggity.GameObjects.Enemies
{
    public class Crub : Enemy
    {
        private readonly BitmapImage _crubSpriteLeft = new(new Uri("pack://application:,,,/Sprites/crub_left.png"));
        private readonly BitmapImage _crubSpriteRight = new(new Uri("pack://application:,,,/Sprites/crub_right.png"));

        public Crub((float, float) position) : base(position)
        {
            _enemySpriteLeft = _crubSpriteLeft;
            _enemySpriteRight = _crubSpriteRight;
            _enemyMoveDirectionVector = DirectionVector.Right;
            EnemyMoveSpeed = 1; 
            _enemySprite = new ObjectSprite((X, Y), this)
            {
                ObjectImage = GetBitmapImage()
            };
        }

        protected override BitmapImage GetBitmapImage()
        {
            return _enemyMoveDirectionVector == DirectionVector.Left ? _enemySpriteLeft : _enemySpriteRight;
        }

        protected override void Walk()
        {
            _velocity.Item1 = _enemyMoveDirectionVector == DirectionVector.Left ? -EnemyMoveSpeed : EnemyMoveSpeed;
        }
    }
}
