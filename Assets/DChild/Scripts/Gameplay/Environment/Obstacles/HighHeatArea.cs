using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class HighHeatArea : DPSObstacle
    {
        [SerializeField,LockAttackType(AttackType.Fire)]
        private AttackDamage m_attackDamage;

        protected override AttackDamage damage => m_attackDamage;
    }
}
