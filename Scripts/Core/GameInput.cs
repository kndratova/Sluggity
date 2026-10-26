using System.Collections.Generic;
using System.Windows.Input;

namespace Sluggity.Core
{
    public class GameInput
    {
        private readonly HashSet<Key> m_heldKeys = new HashSet<Key>();
        private readonly HashSet<Key> m_pressedKeys = new HashSet<Key>();
    
        public int Direction => (Right ? 1 : 0) - (Left ? 1 : 0);
        public bool Left => m_heldKeys.Contains(Key.Left) || m_heldKeys.Contains(Key.A);
        public bool Right => m_heldKeys.Contains(Key.Right) || m_heldKeys.Contains(Key.D);
        public bool JumpHeld => m_heldKeys.Contains(Key.Up) || m_heldKeys.Contains(Key.W) || m_heldKeys.Contains(Key.Space);
        public bool JumpPressed => m_pressedKeys.Contains(Key.Up) || m_pressedKeys.Contains(Key.W) || m_pressedKeys.Contains(Key.Space);
    
        public void Press(Key key)
        {
            if (m_heldKeys.Add(key))
            {
                m_pressedKeys.Add(key);
            }
        }
    
        public void Release(Key key)
        {
            m_heldKeys.Remove(key);
        }

        public void EndFrame()
        {
            m_pressedKeys.Clear();
        }

        public void Reset()
        {
            m_heldKeys.Clear();
            m_pressedKeys.Clear();
        }
    }
}