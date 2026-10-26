using System;

namespace Sluggity.Gameplay.Player
{
    [Flags]
    public enum Buff
    {
        None = 0,
        Knife = 1 << 0,
        Legs = 1 << 1,
    }
}