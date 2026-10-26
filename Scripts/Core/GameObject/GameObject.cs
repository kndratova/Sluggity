using System.Windows.Media;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Core.GameObject
{
    public abstract class GameObject
    {
        protected GameObject(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; protected set; }
        public double Height { get; protected set; }
        
        public Box Bounds => new Box(X, Y, Width, Height);
        
        public bool IsDestroyed { get; private set; }

        // visuals
        public ObjectView View { get; protected set; } = new RectangleView(10, 10, Colors.White);

        public virtual void Draw(Camera camera)
        {
            View.Draw(this, camera);
        }
        
        // life cycle
        public virtual void Start(Scene scene)
        {
            
        }
        
        public virtual void Update(Scene scene, double deltaTime)
        {
            
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }
        
        // helpers
        public bool Overlaps(GameObject other)
        {
            return Bounds.Overlaps(other.Bounds);
        }
    }
}
