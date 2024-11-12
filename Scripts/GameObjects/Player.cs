using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Sluggity.Core;
using Sluggity.GameObjects.Bonuses;

namespace Sluggity.GameObjects
{
    internal class Player : GameObject, ICollidableObject<Player>
    {
        #region Additional
        internal enum PlayerState
        {
            WalkingNormal,
            WalkingKnife,
            WalkingLegs,
            WalkingCombo,
        }

        private readonly Dictionary<PlayerState, BitmapImage> _playerStateSpritesLeft = new()
        {
            { PlayerState.WalkingNormal, new BitmapImage(new Uri("pack://application:,,,/Sprites/mike_left.png")) },
            { PlayerState.WalkingKnife, new BitmapImage(new Uri("pack://application:,,,/Sprites/mike_knife_left.png")) },
            { PlayerState.WalkingLegs, new BitmapImage(new Uri("pack://application:,,,/Sprites/mike_jump_left.png")) },
            { PlayerState.WalkingCombo, new BitmapImage(new Uri("pack://application:,,,/Sprites/mike_ulta_left.png")) },
        };

        private readonly Dictionary<PlayerState, BitmapImage> _playerStateSpritesRight = new()
        {
            { PlayerState.WalkingNormal, new BitmapImage(new Uri("pack://application:,,,/Sprites/mike_right.png")) },
            { PlayerState.WalkingKnife, new BitmapImage(new Uri("pack://application:,,,/Sprites/mike_knife_right.png")) },
            { PlayerState.WalkingLegs, new BitmapImage(new Uri("pack://application:,,,/Sprites/mike_jump_right.png")) },
            { PlayerState.WalkingCombo, new BitmapImage(new Uri("pack://application:,,,/Sprites/mike_ulta_right.png")) }
        };
        #endregion

        private ObjectSprite playerSprite;
        private PlayerState _playerState;
        private DirectionVector _playerMoveDirectionVector;
        private float gravityVelocity = -16f;
        private (float, float) velocity;

        public Player() : base((0, 0))
        {
            _playerState = PlayerState.WalkingNormal;
            _playerMoveDirectionVector = DirectionVector.Right;
            playerSprite = new ObjectSprite(position: (X, Y), this)
            {
                ObjectImage = GetBitmapImage()
            };
            SelfCollider = this;
        }

        private int _playerMoveSpeed = 10;
        private void Walk()
        {
            if (Keyboard.IsKeyDown(Key.Right)) ChangeMoveDirection(DirectionVector.Right);
            else if (Keyboard.IsKeyDown(Key.Left)) ChangeMoveDirection(DirectionVector.Left);
            else return;

            if (_directionsCollision[_playerMoveDirectionVector]) return;
            Console.Write(_playerMoveDirectionVector);

            velocity.Item1 = _playerMoveDirectionVector switch
            {
                DirectionVector.Left => velocity.Item1 -= _playerMoveSpeed,
                DirectionVector.Right => velocity.Item1 += _playerMoveSpeed,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void ApplyGravity()
        {
            velocity.Item2 += gravityVelocity;
        }

        private void ChangeMoveDirection(DirectionVector moveDirectionVector)
        {
            if (moveDirectionVector == _playerMoveDirectionVector) return;
            _playerMoveDirectionVector = moveDirectionVector;
        }

        private BitmapImage GetBitmapImage()
        {
            var bitmapImage = _playerMoveDirectionVector switch
            {
                DirectionVector.Left => _playerStateSpritesLeft[_playerState],
                DirectionVector.Right => _playerStateSpritesRight[_playerState],
                _ => throw new ArgumentOutOfRangeException()
            };

            return bitmapImage;
        }


        private bool _isOnGround => _directionsCollision[DirectionVector.Down];

        private void ChangePlayerState(PlayerState playerState)
        {
            _playerState = playerState;
        }

        private bool _isJumping = false;
        private float _jumpVelocity = 45f;
        private float _jumpTemp;
        private bool _jumpPressedLastFrame;

        private void Jump()
        {
            if (_isJumping && (_isOnGround || _directionsCollision[DirectionVector.Up]) || _jumpTemp < 0)
            {
                _isJumping = false;
                _jumpTemp = 0;
            }


            if ((_isOnGround || _isJumping && _playerState == PlayerState.WalkingLegs) && Keyboard.IsKeyDown(Key.Up) && !_jumpPressedLastFrame)
            {
                _jumpTemp = _jumpVelocity;
                _isJumping = true;
            }
            _jumpPressedLastFrame = Keyboard.IsKeyDown(Key.Up);
            if (_isJumping)
            {
                velocity.Item2 += _jumpTemp;
                _jumpTemp -= 2;
            }
        }

        
        public override void Update()
        {
            playerSprite.Update();
            velocity = (0, 0);
            ApplyGravity();
            Walk();
            Jump();
            GameCore.GameCanvas.RenderTransform = new TranslateTransform(GameCore.GameCanvas.RenderTransformOrigin.X, Y - 200);

            var moveDistanceY = 0f;
            
            switch (velocity.Item2)
            {
                case < 0:
                {
                    while (moveDistanceY < Math.Abs(velocity.Item2) && !_directionsCollision[DirectionVector.Down]) { CheckCollision(); Y--; moveDistanceY++; }

                    break;
                }
                case > 0:
                {
                    while (moveDistanceY < Math.Abs(velocity.Item2) && !_directionsCollision[DirectionVector.Up]) { CheckCollision(); Y++; moveDistanceY++; }

                    break;
                }
            }
            
            var moveDistanceX = 0f;
            switch (velocity.Item1)
            {
                case < 0:
                {
                    while (moveDistanceX < Math.Abs(velocity.Item1) && !_directionsCollision[DirectionVector.Left]) { CheckCollision(); X--; moveDistanceX++; }

                    break;
                }
                case > 0:
                {
                    while (moveDistanceX < Math.Abs(velocity.Item1) && !_directionsCollision[DirectionVector.Right]) { CheckCollision(); X++; moveDistanceX++; }

                    break;
                }
            }



            if (Keyboard.IsKeyDown(Key.F1)) ChangePlayerState(PlayerState.WalkingNormal);
            if (Keyboard.IsKeyDown(Key.F2)) ChangePlayerState(PlayerState.WalkingKnife);
            if (Keyboard.IsKeyDown(Key.F3)) ChangePlayerState(PlayerState.WalkingLegs);
            if (Keyboard.IsKeyDown(Key.F4)) ChangePlayerState(PlayerState.WalkingCombo);

            playerSprite.ObjectImage = GetBitmapImage();
            base.Update();
        }

        private readonly Dictionary<DirectionVector, bool> _directionsCollision = new()
        {
            { DirectionVector.Down, false },
            { DirectionVector.Up, false },
            { DirectionVector.Left, false },
            { DirectionVector.Right, false }
        };

        public ICollidableObject<Player> SelfCollider { get; }
        public bool IsColliderEnabled { get; set; } = true;
        public void CheckCollision()
        {
            foreach (var direction in _directionsCollision.Keys.ToList())
            {
                _directionsCollision[direction] = false;
            }

            var _currentSceneGO = new List<GameObject>(SceneManager.SceneGameObjects);
            foreach (var gameObject in _currentSceneGO)
            {
                if (gameObject is Pearl pearl)
                {
                    if (SelfCollider.CollidesWith(pearl) != DirectionVector.None)
                    {
                        CollectBonus(pearl);
                    }
                } else if (gameObject != this)
                {
                    var collisionDirection = SelfCollider.CollidesWith(gameObject);
                    if (collisionDirection != DirectionVector.None)
                    {
                        _directionsCollision[collisionDirection] = true;
                    }
                }
            }
        }

        public void InteractWithEnemy(Enemies.Enemy enemy)
        {
            SceneManager.ReloadCurrentScene();
        }

        internal void CollectBonus(IBonus bonus)
        {
            bonus.CollectBonus();
        }
    }

}
