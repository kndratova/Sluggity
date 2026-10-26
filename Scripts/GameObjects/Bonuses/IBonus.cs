namespace Sluggity.GameObjects.Bonuses
{
    internal interface IBonus
    {
        SpriteView SpriteView { get; }

        void CollectBonus();
    }
}
