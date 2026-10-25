using System.Windows.Media.Imaging;
using Sluggity.Core;

namespace Sluggity.GameObjects.Enemies
{
    public abstract class Enemy(double x, double y, double width, double height, BitmapSource sprite)
        : PhysicsObject(x, y, width, height, sprite)
    {
        private const double k_stompTolerance = 20;

        public override void OnPlayerTouch(Player player, Scene scene)
        {
            if (player.HasKnife && player.IsFacing(this))
            {
                Destroy();
                return;
            }

            bool isStomp = player.VelocityY < 0 && player.PreviousY >= Top - k_stompTolerance;
            if (isStomp)
            {
                Destroy();
                player.Bounce();
                return;
            }

            scene.KillPlayer();
        }
    }
}
