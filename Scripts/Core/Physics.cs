using System.Collections.Generic;
using System.Linq;
using Sluggity.GameObjects;

namespace Sluggity.Core;

public class Physics
{
    private readonly IReadOnlyList<GameObject.GameObject> m_gameObjects;

    public Physics(IReadOnlyList<GameObject.GameObject> gameObjects)
    {
        m_gameObjects = gameObjects;
    }

    public IEnumerable<Obstacle> Solids => m_gameObjects.OfType<Obstacle>();

    public bool IsSolidAt(double x, double y)
    {
        return Solids.Any(o => o.Bounds.Contains(x, y));
    }
}