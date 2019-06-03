using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class Poison : DamagingStatusEffect
    {
        [SerializeField, LockAttackType(AttackType.Poison)]
        private AttackDamage m_damage;

        protected override AttackDamage damage => m_damage;
        public override StatusEffectType type => StatusEffectType.Poison;
    }
}