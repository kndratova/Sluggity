using System.Collections.Generic;
using System.Linq;
using Sluggity.Core.Objects;

namespace Sluggity.Core.Physics;

public class Collisions
{
    private readonly IReadOnlyList<GameObject> m_gameObjects;

    public Collisions(IReadOnlyList<GameObject> gameObjects)
    {
        m_gameObjects = gameObjects;
    }

    public IEnumerable<Obstacle> Solids => m_gameObjects.OfType<Obstacle>();

    public bool IsSolidAt(double x, double y)
    {
        return Solids.Any(o => o.Bounds.Contains(x, y));
    }
}