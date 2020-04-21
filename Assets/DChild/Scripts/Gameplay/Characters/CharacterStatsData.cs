using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "CharacterStatsData", menuName = "DChild/Gameplay/Character Stats Data" )]
    public class CharacterStatsData : ScriptableObject
    {
        [SerializeField]
        private int m_maxHealth;
        [SerializeField]
        private AttackDamage m_damage;
        [SerializeField, InlineEditorAttribute]
        private AttackResistanceData m_attackResistance;
        [SerializeField]
        private StatusEffectChanceData m_statusInfliction;
        [SerializeField]
        private StatusEffectChanceData m_statusResistanceData;

        public void Apply(Character character)
        {
            //TODO: ApplyStats
        }
    }
}