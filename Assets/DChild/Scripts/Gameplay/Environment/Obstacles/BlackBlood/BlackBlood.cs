using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public abstract class BlackBlood : DPSObstacle
    {
        [SerializeField]
        private AttackDamage m_damage;

        protected override AttackDamage damage => m_damage;
    }
}