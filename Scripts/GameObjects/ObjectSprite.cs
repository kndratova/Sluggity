using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sluggity.GameObjects
{
    public abstract class ObjectSprite : GameObject
    {
        protected readonly Image Image = new Image { Stretch = Stretch.Fill };

        protected ObjectSprite(double x, double y, double width, double height, BitmapSource sprite)
            : base(x, y, width, height)
        {
            SetSprite(sprite);
        }

        public override FrameworkElement Visual => Image;

        protected void SetSprite(BitmapSource sprite)
        {
            if (Image.Source == sprite)
            {
                return;
            }

            Image.Source = sprite;
            Image.Width = sprite.PixelWidth;
            Image.Height = sprite.PixelHeight;
        }

        public override void Draw()
        {
            Canvas.SetLeft(Image, CenterX - Image.Width / 2);
            Canvas.SetTop(Image, -Y - Image.Height);
        }
    }
}
