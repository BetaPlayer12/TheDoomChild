using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class ProjectileData : ScriptableObject
    {
        [SerializeField]
        private AttackDamage[] m_damage;
        [SerializeField]
        private bool m_hasConstantSpeed;
        [SerializeField, ValidateInput("ValidateExplosion"), PreviewField]
        protected GameObject m_impactFX;

        public AttackDamage[] damage => m_damage;
        public bool hasConstantSpeed => m_hasConstantSpeed;
        public GameObject impactFX { get => m_impactFX; }

        protected virtual bool ValidateExplosion(GameObject newExplosion)
        {
            var hasFX = (m_impactFX?.GetComponent<FX>() ?? null) != null;
            if (hasFX == false)
            {
                m_impactFX = null;
            }
            return hasFX;
        }
    }
}
