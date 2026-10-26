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
    
        private bool m_isStarted;
    
        public GameInput GameInput => m_gameInput;
        public Collisions Collisions => m_collisions;
    
        public IReadOnlyList<GameObject> GameObjects => m_gameObjects;

        public Scene(Canvas canvas, Camera camera, GameInput gameInput)
        {
            m_canvas = canvas;
            m_camera = camera;
            m_gameInput = gameInput;
        
            m_collisions = new Collisions(m_gameObjects);
        }
    
        public void Add(GameObject gameObject)
        {
            m_pendingObjects.Add(gameObject);
            if (gameObject.View == null)
            {
                return;
            }

            m_canvas.Children.Add(gameObject.View.Element);
            if (m_isStarted)
            {
                gameObject.Draw(m_camera);
            }
        }
    
        public void Start()
        {
            m_isStarted = true;
            AddPendingObjects();
            Draw();
        }
    
        public void Update(double deltaTime)
        {
            AddPendingObjects();

            foreach (GameObject gameObject in m_gameObjects)
            {
                gameObject.EarlyUpdate(this, deltaTime);
            }

            foreach (GameObject gameObject in m_gameObjects)
            {
                gameObject.Update(this, deltaTime);
            }

            RemoveDestroyed();
            Draw();
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
            foreach (GameObject gameObject in m_gameObjects)
            {
                if (gameObject.IsDestroyed && gameObject.View != null)
                {
                    m_canvas.Children.Remove(gameObject.View.Element);
                }
            }

            m_gameObjects.RemoveAll(o => o.IsDestroyed);
        }

        private void Draw()
        {
            foreach (GameObject gameObject in m_gameObjects)
            {
                gameObject.Draw(m_camera);
            }
        }
    }
}