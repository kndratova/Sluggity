using System;
using System.Collections.Generic;
using System.Linq;
using Sluggity.Core;

namespace Sluggity.GameObjects.Bonuses
{
    internal class Pearl : GameObject, ICollidableObject<Pearl>, IBonus
    {
        public ICollidableObject<Pearl> SelfCollider => this;
        public bool IsColliderEnabled { get; set; } = true;
        public ObjectSprite Sprite { get; }

        public Pearl() : base((0, 0))
        {
            Sprite = new ObjectSprite(position: (X, Y), this)
            {
                ObjectImage = new(new Uri("pack://application:,,,/Sprites/pearl_1.png"))
            };
            GameCore.GameCanvas.Children.Add(Sprite.ObjectRepresentation);
        }

        public void CollectBonus()
        {
            GameCore.GameCanvas.Children.Remove(ObjectRepresentation);
            GameCore.GameCanvas.Children.Remove(Sprite.ObjectRepresentation);
            SceneManager.SceneGameObjects.Remove(this);
        } 
    }
}