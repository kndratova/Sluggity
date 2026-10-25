using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public class SceneExit(double x, double y) : ObjectSprite(x, y, 120, 110, SpriteLoader.Get("coral"))
    {

        public override void OnPlayerTouch(Player player, Scene scene)
        {
            scene.Complete();
        }
    }
}
