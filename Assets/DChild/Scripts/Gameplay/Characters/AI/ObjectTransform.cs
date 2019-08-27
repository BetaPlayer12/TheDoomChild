using DChild.Gameplay.Characters;
using UnityEngine;

namespace DChild.Gameplay
{
    public struct ObjectTransform
    {
        public ObjectTransform(Vector2 position, HorizontalDirection facing) : this()
        {
            this.position = position;
            this.facing = facing;
        }

        public Vector2 position { get; }
        public HorizontalDirection facing { get; }
    }
}