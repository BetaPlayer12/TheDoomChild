using UnityEngine;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "AOEExplosionData", menuName = "DChild/Gameplay/AOE Explosion Data")]
    public class AOEExplosionData : ScriptableObject
    {
        [SerializeField]
        private Damage[] m_damage;
        [SerializeField]
        private Invulnerability m_ignoreInvulerability;
        [SerializeField, MinValue(1f)]
        private float m_damageRadius;
        [SerializeField]
        private ExplosionData m_explosionData;

        public Damage[] damage => m_damage;
        public Invulnerability ignoreInvulerability => m_ignoreInvulerability;
        public float damageRadius => m_damageRadius;
        public float explosiveRadius => m_explosionData.explosiveRadius;
        public float explosivePower => m_explosionData.explosiveRadius;
    }
}
