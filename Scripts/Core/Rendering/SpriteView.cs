using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sluggity.Core.Rendering
{
    public class SpriteView : ObjectView
    {
        private readonly Image m_image = new Image { Stretch = Stretch.Fill };

        public SpriteView(BitmapSource sprite)
        {
            PivotX = 0.5;
            SetSprite(sprite);
        }

        public SpriteView(string spriteName) : this(SpriteLoader.Get(spriteName))
        {
        }

        public override FrameworkElement Element => m_image;

        public void SetSprite(BitmapSource sprite)
        {
            if (m_image.Source == sprite)
            {
                return;
            }

            m_image.Source = sprite;
            m_image.Width = sprite.PixelWidth;
            m_image.Height = sprite.PixelHeight;
        }

        public void SetSprite(string spriteName)
        {
            SetSprite(SpriteLoader.Get(spriteName));
        }
    }
}