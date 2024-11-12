
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sluggity.Core;

namespace Sluggity.Scripts
{
    internal class PearlCounter
    {
        private Label _counter;

        public PearlCounter()
        {
            if (GameCore.GameCanvas != null)
            {
                _counter = new Label()
                {
                    Content = "0",
                    Foreground = Brushes.White,
                    Margin = new Thickness(101, 275, 0, 251),
                    Width = 90,
                    FontSize = 40,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                };

                GameCore.GameCanvas.Children.Add(_counter);
                Console.Write(_counter.Width);
            }
        }
    }
}
