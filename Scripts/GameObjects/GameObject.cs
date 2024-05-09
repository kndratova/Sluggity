using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public class GameObject((int, int) position)
    {
        private readonly Rectangle _objectRepresentation = new();
        private readonly ImageBrush _objectImageBrush = new();
        private readonly SolidColorBrush _objectColorBrush = new();

        public byte[] ColorData { get; set; }
        public int X { get; set; } = position.Item1;
        public int Y { get; set; } = position.Item2;

        public int Width { get; set; }
        public int Height { get; set; }

        private Color ObjectColor => Color.FromRgb(ColorData[0], ColorData[1], ColorData[2]);

        public BitmapImage ObjectImage { get; set; } = null;

        private void UpdateCanvasRepresentation()
        {
            if (Game.GameCanvas == null || _objectRepresentation == null) return;

            if (ObjectImage != null)
            {
                _objectImageBrush.ImageSource = ObjectImage;
                _objectRepresentation.Fill = _objectImageBrush;
                Width = ObjectImage.PixelWidth;
                Height = ObjectImage.PixelHeight;
                Canvas.SetLeft(_objectRepresentation, X - Width / 2);
            } else
            {
                _objectColorBrush.Color = ObjectColor;
                _objectRepresentation.Fill = _objectColorBrush;
                Canvas.SetLeft(_objectRepresentation, X);
            }

            Canvas.SetBottom(_objectRepresentation, Y);
            _objectRepresentation.Width = Width;
            _objectRepresentation.Height = Height;

            if (!Game.GameCanvas.Children.Contains(_objectRepresentation))
            {
                Game.GameCanvas.Children.Add(_objectRepresentation);
            }
        }

        public virtual void Update()
        {
            UpdateCanvasRepresentation();
        }
    }
}
