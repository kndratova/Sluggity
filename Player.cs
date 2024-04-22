using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Sluggity
{
    public enum Direction
    {
        Up, Down, Left, Right
    }

    public class Player : INotifyPropertyChanged
    {
        private Point _position;
        private float _moveSpeed = 5;
        private float _gravityScale = 9;
        private Point _previousPosition;
        private float _jumpVelocity;
        private Rectangle _playerRectangle;
        private bool _isJumping = false; // Flag to track if the player is currently jumping
        private int _jumpTimer = 0;

        private Canvas GameCanvas;
        private List<Rectangle> obstacles = new List<Rectangle>();

        public Player(Canvas GameCanvas, Rectangle playerRectangle)
        {
            foreach (var rectangle in GameCanvas.Children.OfType<Rectangle>())
            {
                if ((string)rectangle.Tag == "Obstacle")
                {
                    obstacles.Add(rectangle);
                }
            }
            _playerRectangle = playerRectangle;
            this.GameCanvas = GameCanvas;
        }

        public Point Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }

        public void ReturnToPreviousPosition()
        {
            Position = _previousPosition;
        }

        public void Move()
        {
            Point newPosition = Position;

            // Horizontal movement
            if (Keyboard.IsKeyDown(Key.Left))
            {
                newPosition = new Point(Position.X - _moveSpeed, Position.Y);
                while (IsCollision(newPosition))
                {
                    newPosition.X++; // Move left until no collision
                }
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                newPosition = new Point(Position.X + _moveSpeed, Position.Y);
                while (IsCollision(newPosition))
                {
                    newPosition.X--; // Move right until no collision
                }
            }

            // Vertical movement (jumping)
            if (Keyboard.IsKeyDown(Key.Space) && !_isJumping) // Check if not already jumping
            {
                _isJumping = true;
                _jumpTimer = 20; // Set the jump duration (adjust as needed)
                _jumpVelocity = -20; // Reset jump velocity
            }

            if (_isJumping)
            {
                Point jumpPosition = new Point(newPosition.X, Position.Y + _jumpVelocity);
                while (IsCollision(jumpPosition))
                {
                    jumpPosition.Y++; // Move upwards until no collision
                }

                newPosition = jumpPosition;
                _jumpVelocity++; // Apply gravity to the jump
                _jumpTimer--;

                if (_jumpTimer <= 0 || _jumpVelocity >= 0)
                {
                    _isJumping = false; // Reset jump flag when jump ends or reaches maximum height
                }
            } else
            {
                // Apply gravity
                newPosition = new Point(newPosition.X, Position.Y + _gravityScale);
                while (IsCollision(newPosition))
                {
                    newPosition.Y--; // Move downwards until no collision
                }
            }

            Position = newPosition;
        }
        public void AdjustGravity()
        {
            Position = new Point(Position.X, Position.Y + _gravityScale);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsCollision(Point newPosition)
        {
            Rect newPlayerCollider = new Rect(newPosition.X, newPosition.Y, _playerRectangle.Width, _playerRectangle.Height);

            foreach (var obstacle in obstacles)
            {
                Rect obstacleCollider = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);

                if (newPlayerCollider.IntersectsWith(obstacleCollider))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
