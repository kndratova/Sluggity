namespace Sluggity.GameObjects.Bonuses
{
    internal interface IBonus
    {
        ObjectSprite Sprite { get; }

        void CollectBonus();
    }
}
