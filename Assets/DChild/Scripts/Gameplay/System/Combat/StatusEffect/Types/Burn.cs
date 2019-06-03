using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public class Burn : DamagingStatusEffect
    {
        [SerializeField]
        [LockAttackType(AttackType.Fire)]
        private AttackDamage m_damage;

        protected override AttackDamage damage => m_damage;
        public override StatusEffectType type => StatusEffectType.Burn;
    }
}