using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Sluggity.Core.Input;
using Sluggity.Core.Objects;
using Sluggity.Core.Physics;
using Sluggity.Core.Rendering;

namespace Sluggity.Core.SceneManagement
{
    public class Scene
    {
        private readonly Canvas m_canvas;
        private readonly Camera m_camera;
        private readonly GameInput m_gameInput;
        private readonly Collisions m_collisions;

        private readonly List<GameObject> m_gameObjects = new List<GameObject>();
        private readonly List<GameObject> m_pendingObjects = new List<GameObject>();

        public GameInput GameInput => m_gameInput;
        public Collisions Collisions => m_collisions;

        public IReadOnlyList<GameObject> GameObjects => m_gameObjects;

        internal Canvas Canvas => m_canvas;
        internal Camera Camera => m_camera;

        public Scene(Canvas canvas, Camera camera, GameInput gameInput)
        {
            m_canvas = canvas;
            m_camera = camera;
            m_gameInput = gameInput;

            m_collisions = new Collisions(m_gameObjects);
        }

        public T Add<T>(T gameObject) where T : GameObject
        {
            m_pendingObjects.Add(gameObject);
            gameObject.AddToScene(this);
            return gameObject;
        }

        public void Start()
        {
            AddPendingObjects();
            Draw();
        }
        
        public void Update(double deltaTime)
        {
            AddPendingObjects();

            foreach (GameObject gameObject in m_gameObjects)
            {
                gameObject.Update(this, deltaTime);
            }
        }
        
        public void FixedUpdate(double fixedDeltaTime)
        {
            // scripted movement no collisions
            foreach (GameObject gameObject in m_gameObjects)
            {
                gameObject.FixedUpdate(this, fixedDeltaTime);
            }

            // physics bodies movement and collision
            foreach (GameObject gameObject in m_gameObjects)
            {
                gameObject.Body?.Step(this, fixedDeltaTime);
            }

            m_collisions.ResolveContacts();
            RemoveDestroyed();
        }

        public void Draw()
        {
            foreach (GameObject gameObject in m_gameObjects)
            {
                foreach (ObjectView view in gameObject.GetComponents<ObjectView>())
                {
                    view.Draw(m_camera);
                }
            }
        }

        private void AddPendingObjects()
        {
            if (m_pendingObjects.Count == 0)
            {
                return;
            }
            
            List<GameObject> newObjects = new List<GameObject>(m_pendingObjects);
            m_pendingObjects.Clear();

            m_gameObjects.AddRange(newObjects);
            foreach (GameObject gameObject in newObjects)
            {
                gameObject.Start(this);
            }
        }

        private void RemoveDestroyed()
        {
            foreach (GameObject gameObject in m_gameObjects.Where(o => o.IsDestroyed))
            {
                gameObject.RemoveFromScene();
            }

            m_gameObjects.RemoveAll(o => o.IsDestroyed);
        }
    }
}