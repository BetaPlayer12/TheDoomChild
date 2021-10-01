using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Combat
{

    public interface ITarget
    {
        bool isAlive { get; }
        Vector2 position { get; }
        IAttackResistance attackResistance { get; }
        void TakeDamage(int totalDamage, DamageType type);
        void Heal(int health);
        int GetInstanceID();
        bool CompareTag(string tag);
    }
}