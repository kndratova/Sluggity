using System.Collections.Generic;
using System.Linq;
using Sluggity.Core.Objects;

namespace Sluggity.Core.Physics
{
    public class Collisions
    {
        private readonly IReadOnlyList<GameObject> m_gameObjects;

        public Collisions(IReadOnlyList<GameObject> gameObjects)
        {
            m_gameObjects = gameObjects;
        }

        public IEnumerable<Collider> Colliders => m_gameObjects
            .Where(o => !o.IsDestroyed)
            .Select(o => o.Collider)
            .Where(c => c != null);

        public IEnumerable<Collider> Solids => Colliders.Where(c => c.IsSolid);

        public bool IsSolidAt(double x, double y)
        {
            return Solids.Any(c => c.GetBounds().Contains(x, y));
        }

        public IEnumerable<Collider> Overlapping(Box box, Collider except = null)
        {
            return Colliders.Where(c => c != except && c.GetBounds().Overlaps(box));
        }

        // enter/exit events raise
        public void ResolveContacts()
        {
            List<Collider> colliders = Colliders.ToList();

            for (int i = 0; i < colliders.Count; i++)
            {
                for (int j = i + 1; j < colliders.Count; j++)
                {
                    Collider a = colliders[i];
                    Collider b = colliders[j];
                    
                    if (a.IsSolid && b.IsSolid)
                    {
                        continue;
                    }

                    if (!a.GetBounds().Overlaps(b.GetBounds()))
                    {
                        continue;
                    }

                    a.AddContact(b);
                    b.AddContact(a);
                }
            }

            foreach (Collider collider in colliders)
            {
                collider.CommitContacts();
            }
        }
    }
}