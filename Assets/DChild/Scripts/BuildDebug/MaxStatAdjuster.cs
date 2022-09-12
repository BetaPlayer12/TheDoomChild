using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Window
{
    public class MaxStatAdjuster : SerializedMonoBehaviour, ITrackableValue
    {
        [SerializeField]
        private PlayerStats m_reference;
        [SerializeField]
        private PlayerStat m_stat;
        [SerializeField]
        private ICappedStat m_cappedStat;
        [SerializeField, MinValue(1)]
        private int m_baseMaxValue;
        [SerializeField, MinValue(1)]
        private int m_incrementRate;
        [SerializeField, MinValue(1)]
        private int m_maxIncrement;

        private int m_currentIncrement;

        public event EventAction<EventActionArgs> ValueChange;

        public float value => m_currentIncrement + 1;

        public void Increase()
        {
            if (m_currentIncrement < m_maxIncrement)
            {
                m_currentIncrement++;
            }
            else
            {
                m_currentIncrement = m_maxIncrement;
            }
            UpdateMaxValue();
        }

        public void Decrease()
        {
            if (m_currentIncrement < m_maxIncrement)
            {
                m_currentIncrement--;
            }
            else
            {
                m_currentIncrement = m_maxIncrement;
            }
            UpdateMaxValue();
        }

        private void UpdateMaxValue()
        {
            var maxValue = m_baseMaxValue + m_incrementRate * m_currentIncrement;
            m_reference.SetBaseStat(m_stat, maxValue);
            m_cappedStat?.ResetValueToMax();
            ValueChange?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_currentIncrement = 0;
            UpdateMaxValue();
        }
    }
}
