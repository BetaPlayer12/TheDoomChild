using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "CharacterStatsData", menuName = "DChild/Gameplay/Character Stats Data")]
    public class CharacterStatsData : ScriptableObject
    {
        [SerializeField,MinValue(1)]
        private int m_maxHealth;
        [SerializeField,HideLabel,BoxGroup("Attack Damage")]
        private AttackDamage m_damage;
        [SerializeField, InlineEditor, TabGroup("Attack Resistance")]
        private AttackResistanceData m_attackResistance;
        [SerializeField, InlineEditor, TabGroup("Status Infliction")]
        private StatusEffectChanceData m_statusInfliction;
        [SerializeField, InlineEditor, TabGroup("Status Resistance")]
        private StatusEffectChanceData m_statusResistanceData;

        public void Apply(Character character)
        {
            var health = character.GetComponentInChildren<Health>();
            health.SetMaxValue(m_maxHealth);
            health.ResetValueToMax();

            character.GetComponentInChildren<Attacker>()?.SetDamage(m_damage);
            character.GetComponentInChildren<AttackResistance>()?.SetData(m_attackResistance);
            character.GetComponentInChildren<StatusInflictor>()?.SetData(m_statusInfliction);
            character.GetComponentInChildren<StatusEffectResistance>()?.SetData(m_statusResistanceData);
        }
    }
}