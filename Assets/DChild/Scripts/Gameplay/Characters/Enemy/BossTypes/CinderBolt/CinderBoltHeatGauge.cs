using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CinderBoltHeatGauge : MonoBehaviour
    {
        [SerializeField]
        private int m_maxValue;
        private int m_currentValue;

        public event EventAction<EventActionArgs> HeatFull;
        public event EventAction<EventActionArgs> HeatChanged;

        public int maxValue => m_maxValue;
        public int currentValue => m_currentValue;

        public void AddHeat(int value)
        {
            m_currentValue += value;
            m_currentValue = Mathf.Clamp(m_currentValue, 0, m_maxValue);

            HeatChanged?.Invoke(this, EventActionArgs.Empty);

            if (m_currentValue == m_maxValue)
            {
                HeatFull?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void Reset()
        {
            m_currentValue = 0;
            HeatChanged?.Invoke(this, EventActionArgs.Empty);
        }
    }
}