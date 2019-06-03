using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class AcidPond : DPSObstacle
    {
        [SerializeField, LockAttackType(AttackType.Physical)]
        private AttackDamage m_damage;

        protected override AttackDamage damage => m_damage;
    }
}

