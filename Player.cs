using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Sluggity
{
    public class Player : INotifyPropertyChanged
    {
        private Point _position;
        private float _moveSpeed = 4;
        private float _gravityScale = 1f;
        private float _jumpForce = 16;
        private float _verticalVelocity = 0f;
        private bool _isOnGround = false;
        private bool _upKeyReleased;
        private Rectangle _playerRectangle;
        private List<Rectangle> obstacles = new List<Rectangle>();

        public Player(Canvas gameCanvas, Rectangle playerRectangle)
        {
            foreach (var rectangle in gameCanvas.Children.OfType<Rectangle>())
            {
                if ((string)rectangle.Tag == "Obstacle")
                {
                    obstacles.Add(rectangle);
                }
            }
            _playerRectangle = playerRectangle;
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

        public void Move()
        {
            ApplyGravity(); // Apply gravity before moving
            Point newPosition = Position; // Store the new position initially as the current position

            if (Keyboard.IsKeyDown(Key.Left))
            {
                newPosition = new Point(newPosition.X - _moveSpeed, newPosition.Y);
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
                newPosition = new Point(newPosition.X + _moveSpeed, newPosition.Y);
            }
            if (Keyboard.IsKeyDown(Key.Up) && _isOnGround && _upKeyReleased)
            {
                _upKeyReleased = false;
                Jump();
            }

            if (Keyboard.IsKeyUp(Key.Up)) _upKeyReleased = true;

            if (!IsCollision(newPosition))
            {
                Position = newPosition;
            }
        }

        private void ApplyGravity()
        {
            // Apply vertical velocity due to gravity
            _verticalVelocity += _gravityScale;
            Point newPosition = new Point(Position.X, Position.Y + _verticalVelocity);
            if (!IsCollision(newPosition))
            {
                _isOnGround = false;
                Position = newPosition;
            } else
            {
                _isOnGround = true;
                _verticalVelocity = 0;
            }
        }

        private void Jump()
        {
            // Apply upward force to initiate jump
            _verticalVelocity = -_jumpForce;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
