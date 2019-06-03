using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class AcidFalls : Obstacle
    {
        [SerializeField,LockAttackType(AttackType.Physical)]
        private AttackDamage m_damage;

        protected override AttackDamage damage => m_damage;
    }
}

