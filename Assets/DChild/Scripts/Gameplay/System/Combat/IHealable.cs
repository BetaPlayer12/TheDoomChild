using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IHealable
    {
        Vector2 position { get; }
        void Heal(int health);
    }
}