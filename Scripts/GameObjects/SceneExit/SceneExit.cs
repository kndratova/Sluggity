using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sluggity.Core;

namespace Sluggity.GameObjects.Enemies
{
    public class SceneExit : GameObject, ICollidableObject<SceneExit>
    {
        private readonly BitmapImage _image = new(new Uri("pack://application:,,,/Sprites/finish_1.png"));
        private readonly ObjectSprite _sprite;
        private Rectangle _colliderVisual;

        public SceneExit((float, float) position) : base(position)
        {
            _sprite = new ObjectSprite((X, Y), this)
            {
                ObjectImage = _image
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


        public override void Update()
        {
            UpdateColliderVisual();
            CheckCollision();
            base.Update();
        }


        private void UpdateColliderVisual()
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

        public ICollidableObject<SceneExit> SelfCollider => this;
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
                            SceneManager.LoadLevelSelection();
                        }
                    }
                }
            }
        }
    }
}
