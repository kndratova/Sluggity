using System.Windows.Controls;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public class Player : PhysicsObject
    {
        private const double k_moveSpeed = 330;
        private const double k_jumpSpeed = 980;
        private const double k_jumpCutSpeed = 350;
        private const double k_bounceSpeed = 700;
        private const double k_coyoteTime = 0.1;
        private const double k_jumpBufferTime = 0.12;

        private const double k_bodyWidth = 120;
        private const double k_bodyHeight = 44;
        private const double k_heightWithLegs = 104;
        private const double k_spriteBodyWidth = 139;

        private double m_coyoteTimer;
        private double m_jumpBufferTimer;
        private bool m_usedDoubleJump;

        public Player(double x, double y) : base(x, y, k_bodyWidth, k_bodyHeight, SpriteLoader.Get("mike_right"))
        {
        }

        public bool FacingRight { get; private set; } = true;
        public bool HasKnife { get; private set; }
        public bool HasLegs { get; private set; }
        public double PreviousY { get; private set; }

        public override void Update(Scene scene, double deltaTime)
        {
            PreviousY = Y;
            GameInput input = scene.Input;

            int direction = 0;
            if (input.Right)
            {
                direction++;
            }

            if (input.Left)
            {
                direction--;
            }

            VelocityX = direction * k_moveSpeed;
            if (direction != 0)
            {
                FacingRight = direction > 0;
            }

            m_coyoteTimer = IsGrounded ? k_coyoteTime : m_coyoteTimer - deltaTime;
            m_jumpBufferTimer = input.JumpPressed ? k_jumpBufferTime : m_jumpBufferTimer - deltaTime;

            if (IsGrounded)
            {
                m_usedDoubleJump = false;
            }

            if (m_jumpBufferTimer > 0)
            {
                if (m_coyoteTimer > 0)
                {
                    Jump();
                }
                else if (HasLegs && !m_usedDoubleJump)
                {
                    m_usedDoubleJump = true;
                    Jump();
                }
            }

            if (!input.JumpHeld && VelocityY > k_jumpCutSpeed)
            {
                VelocityY = k_jumpCutSpeed;
            }

            ApplyGravity(deltaTime);
            Move(scene, deltaTime);
            UpdateSprite();
        }

        public void Bounce()
        {
            VelocityY = k_bounceSpeed;
        }

        public void GiveKnife()
        {
            HasKnife = true;
        }

        public void GiveLegs()
        {
            HasLegs = true;
            Height = k_heightWithLegs;
        }

        public bool IsFacing(GameObject other)
        {
            return FacingRight == other.CenterX > CenterX;
        }

        public override void Draw()
        {
            double bodyLeft = CenterX - k_spriteBodyWidth / 2;
            Canvas.SetLeft(Image, FacingRight ? bodyLeft : bodyLeft + k_spriteBodyWidth - Image.Width);
            Canvas.SetTop(Image, -Y - Image.Height);
        }

        private void Jump()
        {
            VelocityY = k_jumpSpeed;
            m_coyoteTimer = 0;
            m_jumpBufferTimer = 0;
        }

        private void UpdateSprite()
        {
            string name = "mike";
            if (HasKnife && HasLegs)
            {
                name = "mike_combo";
            }
            else if (HasKnife)
            {
                name = "mike_knife";
            }
            else if (HasLegs)
            {
                name = "mike_legs";
            }

            SetSprite(SpriteLoader.Get(name + (FacingRight ? "_right" : "_left")));
        }
    }
}
