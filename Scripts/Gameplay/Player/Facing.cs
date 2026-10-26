namespace Sluggity.Gameplay.Player
{
    public enum Facing
    {
        Left,
        Right,
    }
    
    public static class FacingExtensions
    {
        public static float ToSign(this Facing facing) => facing == Facing.Right ? 1f : -1f;
    }
}