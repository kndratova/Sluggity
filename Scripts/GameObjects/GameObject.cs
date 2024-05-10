using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sluggity.Core;

namespace Sluggity.GameObjects
{
    public class GameObject((float, float) position)
    {
        protected readonly Rectangle ObjectRepresentation = new();
        private readonly SolidColorBrush _objectColorBrush = new();

        public byte[] ColorData { get; set; }
        public float X { get; set; } = position.Item1;
        public float Y { get; set; } = position.Item2;

        public float Width { get; set; }
        public float Height { get; set; }

        private Color ObjectColor => Color.FromRgb(ColorData[0], ColorData[1], ColorData[2]);

        private void UpdateCanvasRepresentation()
        {
            if (GameCore.GameCanvas == null || ObjectRepresentation == null) return;

            if (ColorData != null)
            {
                _objectColorBrush.Color = ObjectColor;
                ObjectRepresentation.Fill = _objectColorBrush;
            }

            Canvas.SetLeft(ObjectRepresentation, X);
            Canvas.SetBottom(ObjectRepresentation, Y);
            ObjectRepresentation.Width = Width;
            ObjectRepresentation.Height = Height;

            if (!GameCore.GameCanvas.Children.Contains(ObjectRepresentation))
            {
                GameCore.GameCanvas.Children.Add(ObjectRepresentation);
            }
        }

        public virtual void Update()
        {
            UpdateCanvasRepresentation();
        }
    }
}
