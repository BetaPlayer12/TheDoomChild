using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class ProjectileData : ScriptableObject
    {
        [SerializeField]
        private AttackDamage[] m_damage;
        [SerializeField]
        private bool m_hasConstantSpeed;

        public AttackDamage[] damage => m_damage;
        public bool hasConstantSpeed => m_hasConstantSpeed;
    }
}
