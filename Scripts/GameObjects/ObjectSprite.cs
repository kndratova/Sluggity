using System.Drawing;
using Sluggity.GameObjects;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Sluggity.Scripts.GameObjects
{
    internal class ObjectSprite((float, float) position, GameObject parentObject) : GameObject(position)
    {
        public BitmapImage ObjectImage { get; set; }
        private readonly ImageBrush _objectImageBrush = new();
        private GameObject gameObject = parentObject;
        private int offsetY = 8;
        public override void Update()
        {
            base.Update();
            _objectImageBrush.ImageSource = ObjectImage;
            ObjectRepresentation.Fill = _objectImageBrush;
            X = gameObject.X + gameObject.Width/2;
            Y = gameObject.Y - offsetY;
            Width = ObjectImage.PixelWidth;
            Height = ObjectImage.PixelHeight;
            Canvas.SetLeft(ObjectRepresentation, X - Width / 2);
            Canvas.SetBottom(ObjectRepresentation, Y);
        }
    }
}