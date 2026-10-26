using System.Windows.Media.Imaging;
using Sluggity.Core.Rendering;

namespace Sluggity.Gameplay.Player
{
    public class PlayerView : SpriteView
    {
        public PlayerView(BitmapSource sprite) : base(sprite)
        {
        }
        public PlayerView(string spriteName) : base(spriteName)
        {
        }
    }
}