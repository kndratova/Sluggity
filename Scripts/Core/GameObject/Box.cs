namespace Sluggity.Core.GameObject;

public readonly struct Box
{
    public Box(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public double X { get; }
    public double Y { get; }
    public double Width { get; }
    public double Height { get; }

    public double Right => X + Width;
    public double Top => Y + Height;
    public double CenterX => X + Width / 2;
    public double CenterY => Y + Height / 2;

    public bool Overlaps(Box other)
    {
        return X < other.Right && Right > other.X && Y < other.Top && Top > other.Y;
    }
}