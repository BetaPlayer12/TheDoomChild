using Holysoft.Event;
using Holysoft.Gameplay;
using UnityEngine;
using static DChild.Gameplay.Combat.Damageable;

namespace DChild.Gameplay.Combat
{
    public interface IDamageable
    {
        Vector2 position { get; }
        Transform transform { get; } //Gian Edit
        bool isAlive { get; }
        ICappedStatInfo health { get; }
        IAttackResistance attackResistance { get; }
        void TakeDamage(int totalDamage, DamageType type);
        void BlockDamage(int totalDamage, DamageType type);
        void SetHitboxActive(bool enable);
        Hitbox[] GetHitboxes();
        void SetInvulnerability(Invulnerability level);
        event EventAction<DamageEventArgs> DamageTaken;
        event EventAction<DamageEventArgs> DamageBlock;
        event EventAction<EventActionArgs> Destroyed;
        int GetInstanceID();
        bool CompareTag(string tag);
    }
}