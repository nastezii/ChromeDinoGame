﻿
namespace ChromeDinoGame.Entities
{
    class Dino : Entity
    {
        private const string _rinningGifPath = "pack://application:,,,/Resources/dino_run.gif";
        private const string _crouchingGifPath = "pack://application:,,,/Resources/dino_crouch.gif";
        private const string _startImagePath = "pack://application:,,,/Resources/dino_start.png";
        private const string _endImagePath = "pack://application:,,,/Resources/dino_dead.png";
        public bool IsRunning { get; private set; } = true;
        public bool IsJumping { get; private set; } = false;
        public bool IsCrouching { get; private set; } = false;

        private readonly double _lineOfGround;
        private double _initialJumpSpeed;
        private double _gravity = 0.5;

        public Dino(double canvasWidth, double canvasHeight, double lineOfGround, double speed)
        {
            SetRunningSprite();

            _lineOfGround = lineOfGround;
            Speed = _initialJumpSpeed = speed * 2;
            PosY = lineOfGround;
            PosX = 50;
        }

        public override void MoveObject()
        {
            if (IsJumping)
            {
                if (PosY + Speed <= _lineOfGround)
                {
                    IsJumping = false;
                    Speed = _initialJumpSpeed;
                    PosY = _lineOfGround;
                }
                else
                {
                    PosY += Speed;
                    Speed -= _gravity;
                }
            }
        }

        public void Jump()
        {
            IsRunning = false;
            IsCrouching = false;

            if (!IsJumping)
            {
                IsJumping = true;
                SetStartSprite();

                MoveObject();
            }
        }

        public void Crouch()
        {
            if (!IsJumping)
            {
                IsRunning = false;
                IsCrouching = true;
                SetCrouchSprite();
            }
        }

        public void Run()
        {
            if (!IsJumping)
            {
                IsCrouching = false;
                IsRunning = true;
                SetRunningSprite();
            }
        }

        public void Dead()
        {
            IsCrouching = false;
            IsJumping = false;
            IsRunning = false;
            SetEndSprite();
        }

        private void SetRunningSprite() => SetSpriteCharacteristics(_rinningGifPath, true);
        private void SetCrouchSprite() => SetSpriteCharacteristics(_crouchingGifPath, true);
        private void SetStartSprite() => SetSpriteCharacteristics(_startImagePath, false);
        private void SetEndSprite() => SetSpriteCharacteristics(_endImagePath, false);
    }
}
