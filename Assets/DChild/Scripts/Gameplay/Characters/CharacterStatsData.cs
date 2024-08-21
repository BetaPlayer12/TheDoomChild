using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "CharacterStatsData", menuName = "DChild/Gameplay/Character/Stats Data")]
    public class CharacterStatsData : ScriptableObject
    {
        [SerializeField]
        private int m_maxHealth;
        [SerializeField, InlineEditor]
        private AttackData m_damage;
        [SerializeField, InlineEditor]
        private AttackResistanceData m_attackResistance;
        [SerializeField, InlineEditor]
        private StatusEffectChanceData m_statusInfliction;
        [SerializeField, InlineEditor]
        private StatusEffectChanceData m_statusResistanceData;

        public int maxHealth => m_maxHealth;
        public AttackData damage => m_damage;
        public AttackResistanceData attackResistance => m_attackResistance;
        public StatusEffectChanceData statusInfliction => m_statusInfliction;
        public StatusEffectChanceData statusResistanceData => m_statusResistanceData;

        public void SetAttackData(AttackData damage)
        {
            m_damage = damage;
        }
    }
}