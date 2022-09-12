using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    [AddComponentMenu("DChild/Gameplay/Combat/Threshold Health")]
    public class ThresholdHealth : Health
    {
        [SerializeField, MinValue(0f), OnValueChanged("SendMaxValue")]
        private int m_maxCountHealth;
        [SerializeField]
        private Holysoft.Collections.RangeInt m_threshold;

        public override int maxValue => m_maxCountHealth;

        public override void SetMaxValue(int value)
        {
            m_maxCountHealth = value;
            m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxCountHealth);
            m_percentHealth = m_currentHealth == 0 ? 0 : (m_currentHealth / m_maxCountHealth);
            base.SetMaxValue(value);
            Debug.Log(m_currentHealth);
        }

        public override void ReduceCurrentValue(int damage)
        {
            if (damage > 0)
            {
                var toApply = Mathf.CeilToInt(damage / (float)m_threshold.max);
                base.ReduceCurrentValue(toApply);
            }
        }
    }
}