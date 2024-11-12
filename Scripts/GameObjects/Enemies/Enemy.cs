using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sluggity.Core;

namespace Sluggity.GameObjects.Enemies
{
    public abstract class Enemy : GameObject, ICollidableObject<Enemy>
    {
        protected BitmapImage _enemySpriteLeft;
        protected BitmapImage _enemySpriteRight;
        protected DirectionVector _enemyMoveDirectionVector;
        protected int EnemyMoveSpeed = 1;
        protected (float, float) _velocity;
        private Rectangle _colliderVisual;
        protected ObjectSprite _enemySprite;

        public Enemy((float, float) position) : base(position)
        {
            _enemySprite = new ObjectSprite((X, Y), this)
            {
                ObjectImage = GetBitmapImage()
            };
            IsColliderEnabled = true;

            _colliderVisual = new Rectangle
            {
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                Fill = Brushes.Transparent
            };
            GameCore.GameCanvas.Children.Add(_colliderVisual);
        }

        protected abstract void Walk();

        protected void ChangeMoveDirection(DirectionVector moveDirectionVector)
        {
            _enemyMoveDirectionVector = moveDirectionVector;
        }

        protected abstract BitmapImage GetBitmapImage();

        public override void Update()
        {
            if (_enemyMoveDirectionVector is DirectionVector.Left or DirectionVector.Right)
            {
                if (_directionsCollision[DirectionVector.Right]) ChangeMoveDirection(DirectionVector.Left);
                if (_directionsCollision[DirectionVector.Left]) ChangeMoveDirection(DirectionVector.Right);
            }
            if (_enemyMoveDirectionVector is DirectionVector.Up or DirectionVector.Down)
            {
                if (_directionsCollision[DirectionVector.Up]) ChangeMoveDirection(DirectionVector.Down);
                if (_directionsCollision[DirectionVector.Down]) ChangeMoveDirection(DirectionVector.Up);
            }

            _enemySprite.Update();
            Walk();
            ProcessMovement();
            _enemySprite.ObjectImage = GetBitmapImage();
            UpdateColliderVisual();
            base.Update();
        }

        private void ProcessMovement()
        {
            if (_enemyMoveDirectionVector is DirectionVector.Left or DirectionVector.Right)
            {
                var moveDistanceX = 0f;
                switch (_velocity.Item1)
                {
                    case < 0:
                        while (moveDistanceX < Math.Abs(_velocity.Item1) && !_directionsCollision[DirectionVector.Left]) { CheckCollision(); X--; moveDistanceX++; }
                        break;
                    case > 0:
                        while (moveDistanceX < Math.Abs(_velocity.Item1) && !_directionsCollision[DirectionVector.Right]) { CheckCollision(); X++; moveDistanceX++; }
                        break;
                }
            }
            if (_enemyMoveDirectionVector is DirectionVector.Up or DirectionVector.Down)
            {
                var moveDistanceY = 0f;
                switch (_velocity.Item2)
                {
                    case < 0:
                        while (moveDistanceY < Math.Abs(_velocity.Item2) && !_directionsCollision[DirectionVector.Down]) { CheckCollision(); Y--; moveDistanceY++; }
                        break;
                    case > 0:
                        while (moveDistanceY < Math.Abs(_velocity.Item2) && !_directionsCollision[DirectionVector.Up]) { CheckCollision(); Y++; moveDistanceY++; }
                        break;
                }
            }           
        }

        protected void UpdateColliderVisual()
        {
            if (_colliderVisual != null)
            {
                _colliderVisual.Width = Width;
                _colliderVisual.Height = Height;
                Canvas.SetLeft(_colliderVisual, X);
                Canvas.SetBottom(_colliderVisual, Y);
            }
        }

        private readonly Dictionary<DirectionVector, bool> _directionsCollision = new()
    {
        { DirectionVector.Down, false },
        { DirectionVector.Up, false },
        { DirectionVector.Left, false },
        { DirectionVector.Right, false }
    };

        public void Destroy()
        {
            GameCore.GameCanvas.Children.Remove(ObjectRepresentation);
            GameCore.GameCanvas.Children.Remove(_enemySprite.ObjectRepresentation);
            GameCore.GameCanvas.Children.Remove(_colliderVisual);
        }

        public ICollidableObject<Enemy> SelfCollider => this;
        public bool IsColliderEnabled { get; set; } = true;

        public void CheckCollision()
        {
            foreach (var direction in _directionsCollision.Keys.ToList())
            {
                _directionsCollision[direction] = false;
            }

            foreach (var gameObject in SceneManager.SceneGameObjects)
            {
                if (gameObject != this)
                {
                    var collisionDirection = SelfCollider.CollidesWith(gameObject);
                    if (collisionDirection != DirectionVector.None)
                    {
                        _directionsCollision[collisionDirection] = true;

                        if (gameObject is Player player)
                        {
                            player.InteractWithEnemy(this);
                        }
                    }
                }
            }
        }
    }
}
