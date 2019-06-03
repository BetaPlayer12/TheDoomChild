using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public class WhenStatModifier : ISoulSkillModule
    {
        private enum Stats
        {
            HP,
            MP
        }

        private enum Comparison
        {
            Greater,
            Lesser
        }

        [SerializeField, LabelText("Stat")]
        private Stats m_toChange;
        [SerializeField, Wrap(1f, 100f), SuffixLabel("%", overlay: true)]
        private int m_value;
        [SerializeField, LabelText("Stat")]
        private Comparison m_comparison;
        [OdinSerialize]
        private ISoulSkillModule[] m_modules;

        private IPlayer m_reference;
        private bool m_hasAppliedEffect;

        public void AttachTo(IPlayer player)
        {
            if (m_toChange == Stats.HP)
            {
                TryApplyEffect((float)player.health.currentValue / player.health.maxValue);
                player.health.ValueChanged += OnStatChange;
            }
            else
            {
                TryApplyEffect((float)player.magic.currentValue / player.magic.maxValue);
                player.magic.ValueChanged += OnStatChange;
            }
        }

        public void DetachFrom(IPlayer player)
        {
            if (m_toChange == Stats.HP)
            {
                player.health.ValueChanged -= OnStatChange;
            }
            else
            {
                player.magic.ValueChanged -= OnStatChange;
            }
        }

        private bool IsValid(float currentPercent)
        {
            if (m_comparison == Comparison.Greater)
            {
                return currentPercent > m_value;
            }
            else
            {
                return currentPercent < m_value;
            }
        }

        private void OnStatChange(object sender, StatInfoEventArgs eventArgs)
        {
            if (m_hasAppliedEffect)
            {
                if (IsValid(eventArgs.percentValue))
                {
                    for (int i = 0; i < m_modules.Length; i++)
                    {
                        m_modules[i].DetachFrom(m_reference);
                    }
                    m_hasAppliedEffect = false;
                }
            }
            else
            {
                TryApplyEffect(eventArgs.percentValue);
            }
        }

        private void TryApplyEffect(float percentValue)
        {
            if (IsValid(percentValue))
            {
                for (int i = 0; i < m_modules.Length; i++)
                {
                    m_modules[i].AttachTo(m_reference);
                }
                m_hasAppliedEffect = true;
            }
        }
    }
}