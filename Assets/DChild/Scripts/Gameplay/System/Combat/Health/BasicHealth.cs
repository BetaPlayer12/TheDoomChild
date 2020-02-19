using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    [AddComponentMenu("DChild/Gameplay/Combat/Basic Health")]
    public class BasicHealth : Health
    {
        [SerializeField, MinValue(0f), OnValueChanged("SendMaxValue")]
        private int m_maxHealth;

        public override int maxValue => m_maxHealth;
        public override void SetMaxValue(int value)
        {
            m_maxHealth = value;
            m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxHealth);
            m_percentHealth = m_currentHealth == 0 ? 0 : (m_currentHealth / m_maxHealth);
            base.SetMaxValue(value);
        }
    }
}