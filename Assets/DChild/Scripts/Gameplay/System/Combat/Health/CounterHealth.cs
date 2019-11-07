using Sirenix.OdinInspector;
using UnityEngine;


namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    [AddComponentMenu("DChild/Gameplay/Combat/Counter Health")]
    public class CounterHealth : Health
    {
        [SerializeField, MinValue(0f), OnValueChanged("SendValueEvent")]
        private int m_maxHealth;

        public override int maxValue => m_maxHealth;

        public override void ReduceCurrentValue(int damage)
        {
            base.ReduceCurrentValue(1);
        }
    }
}

