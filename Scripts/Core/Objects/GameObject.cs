using System.Collections.Generic;
using System.Linq;
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
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
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

        internal void Start(Scene scene)
        {
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
        
        protected virtual void OnStart(Scene scene) { }
        protected virtual void OnUpdate(Scene scene, double deltaTime) { }
        protected virtual void OnFixedUpdate(Scene scene, double fixedDeltaTime) { }

        // shortcuts
        public ObjectView View => GetComponent<ObjectView>();
        public Collider Collider => GetComponent<Collider>();
        public PhysicsBody Body => GetComponent<PhysicsBody>();

        public bool HasCollider => Collider != null;
        public bool IsSolid => Collider?.IsSolid ?? false;
    }
}