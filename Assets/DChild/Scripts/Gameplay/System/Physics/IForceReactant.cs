using UnityEngine;

namespace DChild.Gameplay.Physics
{
    public interface IForceReactant
    {
        Transform transform { get; }
        void React(Vector2 origin, Vector2 force);
    }
}
