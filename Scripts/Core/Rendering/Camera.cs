using System.Windows;

namespace Sluggity.Core.Rendering;

public class Camera
{
    public double X { get; set; }
    public double Y { get; set; }
    public double ViewHeight { get; set; }

    public Point WorldToScreen(double worldX, double worldY)
    {
        return new Point(worldX - X, ViewHeight - (worldY - Y));
    }
}