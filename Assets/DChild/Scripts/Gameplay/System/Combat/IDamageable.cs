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
        event EventAction<DamageEventArgs> DamageTaken;
    }
}