﻿using Holysoft.Event;
using UnityEngine;
using static Refactor.DChild.Gameplay.Combat.Damageable;

namespace DChild.Gameplay.Combat
{
    public interface IDamageable
    {
        Vector2 position { get; }
        Transform transform { get; } //Gian Edit
        bool isAlive { get; }
        IAttackResistance attackResistance { get; }
        void TakeDamage(int totalDamage, AttackType type);
        void SetHitboxActive(bool enable);
        void SetInvulnerability(bool isInvulnerable);
        event EventAction<DamageEventArgs> DamageTaken;
        event EventAction<EventActionArgs> Destroyed;
        int GetInstanceID();
        bool CompareTag(string tag);
    }
}