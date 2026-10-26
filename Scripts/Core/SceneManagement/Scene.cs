using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Sluggity.Core.GameObject;
using Sluggity.GameObjects;

namespace Sluggity.Core.SceneManagement;

public class Scene
{
    private readonly Canvas m_canvas;
    private readonly Camera m_camera;
    private readonly Input m_input;
    private readonly Physics m_physics;
    
    private readonly List<GameObject.GameObject> m_gameObjects = new List<GameObject.GameObject>();
    private readonly List<GameObject.GameObject> m_pendingObjects = new List<GameObject.GameObject>();
    
    private bool m_isStarted;
    
    public Input Input => m_input;
    public Physics Physics => m_physics;
    
    public IReadOnlyList<GameObject.GameObject> GameObjects => m_gameObjects;

    public Scene(Canvas canvas, Camera camera, Input input)
    {
        m_canvas = canvas;
        m_camera = camera;
        m_input = input;
        
        m_physics = new Physics(m_gameObjects);
    }
    
    public void Add(GameObject.GameObject gameObject)
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

        foreach (GameObject.GameObject gameObject in m_gameObjects)
        {
            gameObject.EarlyUpdate(this, deltaTime);
        }

        foreach (GameObject.GameObject gameObject in m_gameObjects)
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
        
        List<GameObject.GameObject> newObjects = new List<GameObject.GameObject>(m_pendingObjects);
        m_pendingObjects.Clear();

        m_gameObjects.AddRange(newObjects);
        foreach (GameObject.GameObject gameObject in newObjects)
        {
            gameObject.Start(this);
        }
    }

    private void RemoveDestroyed()
    {
        foreach (GameObject.GameObject gameObject in m_gameObjects)
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
        foreach (GameObject.GameObject gameObject in m_gameObjects)
        {
            gameObject.Draw(m_camera);
        }
    }
}