using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public class ObjectSprite((float, float) position, GameObject parentGameObject) : GameObject(position)
    {
        private const float OffsetY = 8f;
        public BitmapImage ObjectImage { get; set; }

        private readonly ImageBrush _objectImageBrush = new();

        public override void Update()
        {
            base.Update();
            
            _objectImageBrush.ImageSource = ObjectImage;
            ObjectRepresentation.Fill = _objectImageBrush;
            
            X = parentGameObject.X + parentGameObject.Width / 2f;
            Y = parentGameObject.Y - OffsetY;
            
            Width = ObjectImage.PixelWidth;
            Height = ObjectImage.PixelHeight;

            Canvas.SetLeft(ObjectRepresentation, X - Width / 2f);
            Canvas.SetBottom(ObjectRepresentation, Y);
        }
    }
}