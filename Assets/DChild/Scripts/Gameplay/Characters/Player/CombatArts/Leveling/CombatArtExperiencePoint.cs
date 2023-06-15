using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Player.CombatArt.Leveling
{
    public class CombatArtExperiencePoint : MonoBehaviour, ICappedStat
    {
        [SerializeField, MinValue(1)]
        private int m_maxValue = 1;
        [ShowInInspector, HideInEditorMode]
        protected int m_currentValue;

        public int currentValue => m_currentValue;

        public int maxValue => m_maxValue;

        public event EventAction<EventActionArgs> MaxValueReached;
        public event EventAction<StatInfoEventArgs> ValueChanged;
        public event EventAction<StatInfoEventArgs> MaxValueChanged;

        public void AddCurrentValue(int value)
        {
            m_currentValue += value;
            while (m_currentValue >= maxValue)
            {
                m_currentValue -= maxValue;
                MaxValueReached?.Invoke(this, EventActionArgs.Empty);
            }
            ValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentValue, maxValue));
        }

        public void SetCurrentValue(int value)
        {
            m_currentValue = value;
            while (m_currentValue >= maxValue)
            {
                m_currentValue -= maxValue;
                MaxValueReached?.Invoke(this, EventActionArgs.Empty);
            }
            ValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentValue, maxValue));
        }

        public void ReduceCurrentValue(int value)
        {
            m_currentValue -= value;
            if (m_currentValue <= 0)
            {
                m_currentValue = 0;
            }
            ValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentValue, maxValue));
        }

        public void ResetValueToMax()
        {
            throw new System.NotImplementedException();
        }

        public void SetMaxValue(int value)
        {
            m_maxValue = value;
            m_currentValue = Mathf.Clamp(m_currentValue, 0, m_maxValue);
            MaxValueChanged?.Invoke(this, new StatInfoEventArgs(m_currentValue, maxValue));
        }
    }

}