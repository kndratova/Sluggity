using System;

namespace Sluggity.GameObjects
{
    internal interface ICollidableObject<out T> where T : GameObject
    {
        private T Self => this as T;
        public ICollidableObject<T> SelfCollider { get; }
        public bool IsColliderEnabled { get; set; }

        public bool HasIntersection(GameObject other) =>
            other is ICollidableObject<GameObject> { IsColliderEnabled: true } &&
            Self.X < other.X + other.Width &&
            Self.X + Self.Width > other.X &&
            Self.Y < other.Y + other.Height &&
            Self.Y + Self.Height > other.Y;

        public bool HasCollision(GameObject other) =>
            other is ICollidableObject<GameObject> { IsColliderEnabled: true } &&
            Self.X <= other.X + other.Width &&
            Self.X + Self.Width >= other.X &&
            Self.Y <= other.Y + other.Height &&
            Self.Y + Self.Height >= other.Y;


        public DirectionVector CollidesWith(GameObject other)
        {
            return !HasCollision(other) ? DirectionVector.None : GetCollisionDirectionVector(other);
        }

        public DirectionVector IntersectsWith(GameObject other)
        {
            return !HasIntersection(other) ? DirectionVector.None : GetCollisionDirectionVector(other);
        }

        private DirectionVector GetCollisionDirectionVector(GameObject other)
        {
            var x = Self.X + Self.Width / 2.0 - (other.X + other.Width / 2.0);
            var y = Self.Y + Self.Height / 2.0 - (other.Y + other.Height / 2.0);
            
            var width = Self.Width / 2.0 + other.Width / 2.0;
            var height = Self.Height / 2.0 + other.Height / 2.0;
            
            var crossWidth = width * y;
            var crossHeight = height * x;

            if (!(Math.Abs(x) <= width) || !(Math.Abs(y) <= height))
                return DirectionVector.None;


            if (crossWidth > crossHeight) return crossWidth > -crossHeight ? DirectionVector.Down : DirectionVector.Right;

            return crossWidth > -crossHeight ? DirectionVector.Left : DirectionVector.Up;
        }

    }
}
