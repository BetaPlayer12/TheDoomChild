using Refactor.DChild.Gameplay;
using Refactor.DChild.Gameplay.Characters;
using Refactor.DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Combat
{

    public interface ITarget
    {
        bool isAlive { get; }
        Vector2 position { get; }
        IAttackResistance attackResistance { get; }
        void TakeDamage(int totalDamage, AttackType type);
        void Heal(int health);
        int GetInstanceID();
        bool CompareTag(string tag);
    }
}