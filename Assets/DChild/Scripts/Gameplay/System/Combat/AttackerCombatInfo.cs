using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class AttackerCombatInfo
    {
        public void Initialize(GameObject attacker, Vector2 position, int critChance, float critDamageModifier,bool ignoresBlock,params AttackDamage[] damage)
        {
            this.instance = attacker;
            this.position = position;
            this.critChance = critChance;
            this.critDamageModifier = critDamageModifier;
            this.damage = damage;
            hasForceDirection = false;
            this.ignoresBlock = ignoresBlock;
            forceDirection = Vector2.zero;
        }

        public void Initialize(GameObject attacker, Vector2 position, int critChance, float critDamageModifier,bool ignoresBlock,Vector2 forceDirection, params AttackDamage[] damage)
        {
            this.instance = attacker;
            this.position = position;
            this.critChance = critChance;
            this.critDamageModifier = critDamageModifier;
            this.damage = damage;
            hasForceDirection = true;
            this.ignoresBlock = ignoresBlock;
            this.forceDirection = forceDirection;
        }

        public GameObject instance { get; private set; }
        public Vector2 position { get; private set; }
        public int critChance { get; private set; }
        public float critDamageModifier { get; private set; }
        public AttackDamage[] damage { get; private set; }
        public bool hasForceDirection { get; private set; }
        public bool ignoresBlock { get; private set; }
        public Vector2 forceDirection { get; private set; }
    }
}