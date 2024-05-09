using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace Sluggity.GameObjects
{
    internal class Player : GameObject
    {
        #region Additional
        internal enum PlayerState
        {
            WalkingNormal,
            WalkingKnife,
            WalkingLegs,
            WalkingCombo,
        }

        private enum MoveDirection
        {
            Left,
            Right
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

        private PlayerState _playerState;
        private MoveDirection _playerMoveDirection;
        private int _playerMoveSpeed = 5;

        public Player() : base((0, 0))
        {
            _playerState = PlayerState.WalkingNormal;
            _playerMoveDirection = MoveDirection.Right;
            ObjectImage = GetBitmapImage();
        }

        private void Move()
        {
            if (Keyboard.IsKeyDown(Key.Right)) ChangeMoveDirection(MoveDirection.Right);
            else if (Keyboard.IsKeyDown(Key.Left)) ChangeMoveDirection(MoveDirection.Left);
            else return;

            X += _playerMoveSpeed;
        }

        private BitmapImage GetBitmapImage()
        {
            var bitmapImage = _playerMoveDirection switch
            {
                MoveDirection.Left => _playerStateSpritesLeft[_playerState],
                MoveDirection.Right => _playerStateSpritesRight[_playerState],
                _ => throw new ArgumentOutOfRangeException()
            };

            return bitmapImage;
        }


        private void ChangePlayerState(PlayerState playerState)
        {
            if (playerState == _playerState) return;
            _playerState = playerState;
            Console.Write(this +@": YA PEDIC " + playerState);
        }

        private void ChangeMoveDirection(MoveDirection moveDirection)
        {
            if (moveDirection == _playerMoveDirection) return;
            _playerMoveDirection = moveDirection;
            _playerMoveSpeed = -_playerMoveSpeed;
            Console.Write(this + @": YA PEDIC " + moveDirection);
        }

        public override void Update()
        {
            Move();
            if(Keyboard.IsKeyDown(Key.F1)) ChangePlayerState(PlayerState.WalkingNormal);
            if(Keyboard.IsKeyDown(Key.F2)) ChangePlayerState(PlayerState.WalkingKnife);
            if(Keyboard.IsKeyDown(Key.F3)) ChangePlayerState(PlayerState.WalkingLegs);
            if(Keyboard.IsKeyDown(Key.F4)) ChangePlayerState(PlayerState.WalkingCombo);

            ObjectImage = GetBitmapImage();
            base.Update();
        }
    }
}
