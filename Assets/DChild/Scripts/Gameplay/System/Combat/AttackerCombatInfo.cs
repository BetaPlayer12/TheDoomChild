using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct AttackerCombatInfo
    {
        public AttackerCombatInfo(Vector2 position, int critChance, float critDamageModifier, params AttackDamage[] damage) : this()
        {
            this.position = position;
            this.critChance = critChance;
            this.critDamageModifier = critDamageModifier;
            this.damage = damage;
            hasForceDirection = false;
            forceDirection = Vector2.zero;
        }

        public AttackerCombatInfo(Vector2 position, int critChance, float critDamageModifier, Vector2 forceDirection, params AttackDamage[] damage) : this()
        {
            this.position = position;
            this.critChance = critChance;
            this.critDamageModifier = critDamageModifier;
            this.damage = damage;
            hasForceDirection = true;
            this.forceDirection = forceDirection;
        }

        public Vector2 position { get; }
        public int critChance { get; }
        public float critDamageModifier { get; }
        public AttackDamage[] damage { get; }
        public bool hasForceDirection { get; }
        public Vector2 forceDirection { get; }
    }
}