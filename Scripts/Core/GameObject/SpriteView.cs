using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Sluggity.Core.GameObject;
using Sluggity.GameObjects;

public class SpriteView : ObjectView
{
    private readonly Image m_image = new Image { Stretch = Stretch.Fill };

    public SpriteView(BitmapSource sprite)
    {
        SetSprite(sprite);
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

    public override void Draw(GameObject owner, Camera camera)
    {
        Point screen = camera.WorldToScreen(owner.Bounds.CenterX - m_image.Width / 2, owner.Bounds.Y + m_image.Height);
        Canvas.SetLeft(m_image, screen.X);
        Canvas.SetTop(m_image, screen.Y);
    }
}