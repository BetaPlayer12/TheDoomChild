using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public struct AttackInfo
    {
        public AttackInfo(Vector2 position, int critChance, float critDamageModifier, AttackDamage damage) : this()
        {
            this.position = position;
            this.critChance = critChance;
            this.critDamageModifier = critDamageModifier;
            this.damage = damage;
        }

       public Vector2 position { get; }
       public int critChance { get; }
       public float critDamageModifier { get; }
       public AttackDamage damage { get; }
    }
}