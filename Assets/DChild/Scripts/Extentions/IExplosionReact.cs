using UnityEngine;

namespace DChild.Gameplay
{
    public interface IExplosionReact
    {
        Transform transform { get; }
        void React(Vector2 origin, Vector2 force);
    }

}