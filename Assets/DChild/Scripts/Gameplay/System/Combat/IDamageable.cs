using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public interface IDamageable
    {
        Vector2 position { get; }
        Transform transform { get; } //Gian Edit
        bool isAlive { get; }
        IAttackResistance attackResistance { get; }
        void TakeDamage(int totalDamage, AttackType type);
    }
}