using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IKnockable
    {
        void Displace(Vector2 force);
    }
}