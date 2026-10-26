using Sluggity.Core.Objects;
using Sluggity.Core.SceneManagement;

public abstract class Component
{
    public GameObject Owner { get; private set; }

    internal void Attach(GameObject owner)
    {
        Owner = owner;
    }
    
    public virtual void OnAddedToScene(Scene scene) { }
    public virtual void OnRemovedFromScene(Scene scene) { }

    public virtual void Start(Scene scene) { }
    public virtual void Update(Scene scene, double deltaTime) { }
    public virtual void FixedUpdate(Scene scene, double fixedDeltaTime) { }
    public virtual void LateUpdate(Scene scene, double deltaTime) { }
}