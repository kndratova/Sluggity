
// TODO: EDIT COMPLETELY

using System.Runtime.CompilerServices;

namespace Sluggity.GameObjects
{
    internal class Obstacle() : GameObject((0, 0)), ICollidableObject<Obstacle>
    {
        public ICollidableObject<Obstacle>? SelfCollider => this;
        public bool IsColliderEnabled { get; set; } = true;
    }
}
