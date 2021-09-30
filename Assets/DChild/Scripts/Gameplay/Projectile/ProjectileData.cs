using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class ProjectileData : ScriptableObject
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        private Invulnerability m_ignoreInvulnerability;
        [SerializeField]
        private bool m_hasConstantSpeed;
        [SerializeField]
        private bool m_willFaceVelocity;
        [SerializeField]
        private bool m_isGroundProjectile;
        [SerializeField, ValidateInput("ValidateExplosion"), PreviewField]
        protected GameObject m_impactFX;
        [SerializeField, ValidateInput("ValidateExplosion"), PreviewField]
        protected GameObject m_impactCrater;

        public Damage damage => m_damage;
        public Invulnerability ignoreInvulnerability => m_ignoreInvulnerability;
        public bool hasConstantSpeed => m_hasConstantSpeed;

        public bool willFaceVelocity => m_willFaceVelocity;
        public bool isGroundProjectile => m_isGroundProjectile;
        public GameObject impactFX => m_impactFX;
        public GameObject impactCrater => m_impactCrater;

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
