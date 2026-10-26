using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Sluggity.Core.Input
{
    public class GameInput
    {
        private readonly Dictionary<Key, GameAction> m_bindings = new Dictionary<Key, GameAction>
        {
            [Key.Left] = GameAction.MoveLeft,
            [Key.A] = GameAction.MoveLeft,
            [Key.Right] = GameAction.MoveRight,
            [Key.D] = GameAction.MoveRight,
            [Key.Up] = GameAction.Jump,
            [Key.W] = GameAction.Jump,
            [Key.Space] = GameAction.Jump,
            [Key.Escape] = GameAction.Pause,
            [Key.R] = GameAction.Restart,
        };

        private readonly HashSet<Key> m_heldKeys = new HashSet<Key>();
        private readonly HashSet<GameAction> m_pressedActions = new HashSet<GameAction>();

        public event Action<GameAction> ActionPressed;
        public event Action<GameAction> ActionReleased;

        public int Direction => (IsHeld(GameAction.MoveRight) ? 1 : 0) - (IsHeld(GameAction.MoveLeft) ? 1 : 0);

        public bool IsHeld(GameAction action)
        {
            foreach (Key key in m_heldKeys)
            {
                if (m_bindings.TryGetValue(key, out GameAction bound) && bound == action)
                {
                    return true;
                }
            }

            return false;
        }

        public bool WasPressed(GameAction action) => m_pressedActions.Contains(action);

        public void Press(Key key)
        {
            if (!m_heldKeys.Add(key) || !m_bindings.TryGetValue(key, out GameAction action))
            {
                return;
            }

            m_pressedActions.Add(action);
            ActionPressed?.Invoke(action);
        }

        public void Release(Key key)
        {
            if (!m_heldKeys.Remove(key) || !m_bindings.TryGetValue(key, out GameAction action))
            {
                return;
            }

            ActionReleased?.Invoke(action);
        }

        public void EndFrame()
        {
            m_pressedActions.Clear();
        }

        public void Reset()
        {
            m_heldKeys.Clear();
            m_pressedActions.Clear();
        }
    }
}