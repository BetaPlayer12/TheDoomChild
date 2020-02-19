using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class AttackerCombatInfo
    {
        public void Initialize(Vector2 position, int critChance, float critDamageModifier, params AttackDamage[] damage)
        {
            this.position = position;
            this.critChance = critChance;
            this.critDamageModifier = critDamageModifier;
            this.damage = damage;
            hasForceDirection = false;
            forceDirection = Vector2.zero;
        }

        public void Initialize(Vector2 position, int critChance, float critDamageModifier, Vector2 forceDirection, params AttackDamage[] damage)
        {
            this.position = position;
            this.critChance = critChance;
            this.critDamageModifier = critDamageModifier;
            this.damage = damage;
            hasForceDirection = true;
            this.forceDirection = forceDirection;
        }

        public Vector2 position { get; private set; }
        public int critChance { get; private set; }
        public float critDamageModifier { get; private set; }
        public AttackDamage[] damage { get; private set; }
        public bool hasForceDirection { get; private set; }
        public Vector2 forceDirection { get; private set; }
    }
}