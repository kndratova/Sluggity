using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public class Obstacle : GameObject, ICollidableObject<Obstacle>
    {
        private Rectangle _colliderVisual;

        public Obstacle((float, float) position) : base(position)
        {
            IsColliderEnabled = true;

            _colliderVisual = new Rectangle
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                Fill = Brushes.Transparent
            };
            GameCore.GameCanvas.Children.Add(_colliderVisual);
        }

        public ICollidableObject<Obstacle> SelfCollider => this;
        public bool IsColliderEnabled { get; set; }

        public override void Update()
        {
            base.Update();
            UpdateColliderVisual();
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
    }
}
