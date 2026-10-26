using Sluggity.Core.Input;
using Sluggity.Core.Objects;
using Sluggity.Core.Physics;
using Sluggity.Core.Rendering;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Gameplay.Player
{
    public class PlayerCharacter : GameObject
    {
        private const double k_bodyWidth = 100;
        private const double k_bodyHeight = 44;

        private readonly SpriteView m_view;
        private readonly PlayerMovement m_movement;
        private GameInput m_gameInput;

        private Facing m_facing = Facing.Right;

        public PlayerCharacter(double x, double y) : base(x, y)
        {
            m_view = AddComponent(new SpriteView("mike_right"));
            AddComponent(new Collider(-k_bodyWidth/2, 0, k_bodyWidth, k_bodyHeight));
            m_movement = new PlayerMovement(AddComponent(new PhysicsBody()));
        }

        protected override void OnStart(Scene scene)
        {
            m_gameInput = scene.GameInput;
        }

        protected override void OnUpdate(Scene scene, double deltaTime)
        {
            int direction = m_gameInput.Direction;
            m_movement.SetMoveInput(direction);

            if (direction != 0)
            {
                m_facing = direction > 0 ? Facing.Right : Facing.Left;
            }

            if (m_gameInput.WasPressed(GameAction.Jump))
            {
                m_movement.RequestJump();
            }

            m_movement.Update(scene, deltaTime);

            m_view.SetSprite(m_facing == Facing.Right ? "mike_right" : "mike_left");
        }
    }
}
