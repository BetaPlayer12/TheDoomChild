using DChild.Gameplay.Combat;
using UnityEngine;


namespace DChild.Gameplay.Environment.Obstacles
{
    public class SimpleObstacle : Obstacle
    {
        [SerializeField]
        private AttackDamage m_damage;

        protected override AttackDamage damage => m_damage;      
    }
}