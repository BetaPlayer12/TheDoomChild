using Sirenix.OdinInspector;
using UnityEngine;


namespace DChild.Gameplay.Combat
{
    [System.Serializable]

    public class HealthCounterScript : Health
    {
        [SerializeField, MinValue(0f), OnValueChanged("SendValueEvent")]
        private int m_maxHealth;

        public override int maxValue => m_maxHealth;





        public void counterfill(int value)
        {
            m_maxHealth = value;
            m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxHealth);
            m_percentHealth = m_currentHealth == 0 ? 0 : (m_currentHealth / m_maxHealth);
            base.SetMaxValue(value);
        }

        public override void ReduceCurrentValue(int damage)
        {

            base.ReduceCurrentValue(1);
        }

    }
}

