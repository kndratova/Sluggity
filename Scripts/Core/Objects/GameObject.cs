using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Sluggity.Core.Physics;
using Sluggity.Core.Rendering;
using Sluggity.Core.SceneManagement;

namespace Sluggity.Core.Objects
{
    public class GameObject
    {
        private readonly List<Component> m_components = new List<Component>();
        private bool m_isStarted;

        public GameObject(double x, double y)
        {
            MoveInstant(x, y);
        }

        public double X { get; set; }
        public double Y { get; set; }

        // position at the start of the last fixed step
        public double PreviousX { get; private set; }
        public double PreviousY { get; private set; }

        // interpolated position ONLY FOR DRAWING
        public double RenderX { get; private set; }
        public double RenderY { get; private set; }

        public bool IsDestroyed { get; private set; }
        public bool IsStarted => m_isStarted;
        public Scene Scene { get; private set; }

        public T AddComponent<T>(T component) where T : Component
        {
            component.Attach(this);
            m_components.Add(component);

            if (Scene != null)
            {
                component.OnAddedToScene(Scene);
            }

            if (m_isStarted)
            {
                component.Start(Scene);
            }
            
            if (component is Collider collider)
            {
                RectangleView debugRect = AddComponent(new RectangleView(collider.Width, collider.Height, null, Colors.GreenYellow));
                debugRect.OffsetX = collider.OffsetX;
                debugRect.OffsetY = collider.OffsetY;
            }
                
            return component;
        }

        public bool RemoveComponent(Component component)
        {
            if (!m_components.Remove(component))
            {
                return false;
            }

            if (Scene != null)
            {
                component.OnRemovedFromScene(Scene);
            }

            return true;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in m_components)
            {
                if (component is T typed)
                {
                    return typed;
                }
            }

            return null;
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();
            return component != null;
        }

        public IEnumerable<T> GetComponents<T>() where T : Component
        {
            return m_components.OfType<T>();
        }

        public void Destroy()
        {
            IsDestroyed = true;
        }

        internal void AddToScene(Scene scene)
        {
            Scene = scene;

            foreach (Component component in m_components.ToList())
            {
                component.OnAddedToScene(scene);
            }
        }

        internal void RemoveFromScene()
        {
            foreach (Component component in m_components.ToList())
            {
                component.OnRemovedFromScene(Scene);
            }

            Scene = null;
        }

        // moves go without interpolation
        public void MoveInstant(double x, double y)
        {
            X = x;
            Y = y;
            PreviousX = RenderX = x;
            PreviousY = RenderY = y;
        }

        internal void SavePreviousPosition()
        {
            PreviousX = X;
            PreviousY = Y;
        }

        internal void Interpolate(double alpha)
        {
            RenderX = PreviousX + (X - PreviousX) * alpha;
            RenderY = PreviousY + (Y - PreviousY) * alpha;
        }

        internal void Start(Scene scene)
        {
            MoveInstant(X, Y);
            m_isStarted = true;
            OnStart(scene);

            foreach (Component component in m_components.ToList())
            {
                component.Start(scene);
            }
        }

        internal void Update(Scene scene, double deltaTime)
        {
            OnUpdate(scene, deltaTime);

            foreach (Component component in m_components.ToList())
            {
                component.Update(scene, deltaTime);
            }
        }

        internal void FixedUpdate(Scene scene, double fixedDeltaTime)
        {
            OnFixedUpdate(scene, fixedDeltaTime);

            foreach (Component component in m_components.ToList())
            {
                component.FixedUpdate(scene, fixedDeltaTime);
            }
        }
        
        internal void LateUpdate(Scene scene, double deltaTime)
        {
            OnLateUpdate(scene, deltaTime);

            foreach (Component component in m_components.ToList())
            {
                component.LateUpdate(scene, deltaTime);
            }
        }
        
        
        protected virtual void OnStart(Scene scene) { }
        protected virtual void OnUpdate(Scene scene, double deltaTime) { }
        protected virtual void OnFixedUpdate(Scene scene, double fixedDeltaTime) { }
        protected virtual void OnLateUpdate(Scene scene, double deltaTime) { }

        // shortcuts
        public ObjectView View => GetComponent<ObjectView>();
        public Collider Collider => GetComponent<Collider>();
        public PhysicsBody Body => GetComponent<PhysicsBody>();

        public bool HasCollider => Collider != null;
        public bool IsSolid => Collider?.IsSolid ?? false;
    }
}