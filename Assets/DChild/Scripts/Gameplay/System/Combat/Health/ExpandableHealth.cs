using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    public class ExpandableHealth : Health
    {
        [SerializeField]
        [MinValue(DEFAULT_HEALTH)]
        private int m_baseMaxHealth;
        [SerializeField]
        [MinValue(0)]
        private int m_additionalMaxHealth;

        private const int DEFAULT_HEALTH = 30;

        public ExpandableHealth(int baseMaxHealth)
        {
            m_baseMaxHealth = baseMaxHealth;
            m_additionalMaxHealth = 0;
        }

        public override int maxValue => m_baseMaxHealth + m_additionalMaxHealth;

        private int currentHealth
        {
            set
            {
                m_currentHealth = value;
                m_percentHealth = m_currentHealth / (float)maxValue;
            }
        }

        public void InitializeHealth(int value)
        {
            m_baseMaxHealth = DEFAULT_HEALTH + value;
            currentHealth = m_baseMaxHealth;
        }

        public void SetMaxBaseHealth(int value)
        {
            m_baseMaxHealth = DEFAULT_HEALTH + Mathf.Max(0, value);
            currentHealth = Mathf.Min(maxValue, m_currentHealth);
        }

        public void AddMax(int value)
        {
            m_additionalMaxHealth += Mathf.Abs(value);
            currentHealth = Mathf.Min(maxValue, m_currentHealth);
        }

        public void ReduceMax(int value)
        {
            m_additionalMaxHealth = Mathf.Max(0, m_additionalMaxHealth - Mathf.Abs(value));
            currentHealth = Mathf.Min(maxValue, m_currentHealth);
        }

        public override void SetMaxValue(int value)
        {
            throw new System.NotImplementedException();
        }
    }
}