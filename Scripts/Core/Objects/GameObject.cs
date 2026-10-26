using System.Windows.Media;
using Sluggity.Core.Rendering;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Core.Objects
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
        
        public bool IsCollidable { get; protected set; }
        public bool IsDestroyed { get; private set; }

        // visuals
        public ObjectView View { get; protected set; } = new RectangleView(10, 10, Colors.White);

        public virtual void Draw(Camera camera)
        {
            View?.Draw(this, camera);
        }
        
        // life cycle
        public virtual void Start(Scene scene)
        {
            
        }
        
        // runs before update for all objects, for physics etc
        public virtual void EarlyUpdate(Scene scene, double deltaTime)
        {
        }
        
        public virtual void Update(Scene scene, double deltaTime)
        {
            
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }
    }
}
