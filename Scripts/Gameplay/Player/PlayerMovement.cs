using Sluggity.Core.Physics;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Gameplay.Player
{
    public class PlayerMovement
    {
        private const double k_moveSpeed = 400;
        private const double k_jumpSpeed = 1000;

        private readonly PhysicsBody m_body;

        private int m_moveDirection;
        private bool m_jumpRequested;

        public bool IsGrounded => m_body.IsGrounded;

        public PlayerMovement(PhysicsBody body)
        {
            m_body = body;
        }

        public void Update(Scene scene, double deltaTime)
        {
            m_body.VelocityX = m_moveDirection * k_moveSpeed;

            if (m_jumpRequested && IsGrounded)
            {
                m_body.VelocityY = k_jumpSpeed;
            }

            m_jumpRequested = false;
        }

        public void SetMoveInput(int direction)
        {
            m_moveDirection = direction;
        }

        public void RequestJump()
        {
            m_jumpRequested = true;
        }
    }
}
