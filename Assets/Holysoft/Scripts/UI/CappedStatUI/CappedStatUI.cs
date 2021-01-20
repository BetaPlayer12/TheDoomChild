using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Holysoft.Gameplay.UI
{
    public abstract class CappedStatUI : SerializedMonoBehaviour
    {
        //#if UNITY_EDITOR
        [OdinSerialize, OnValueChanged("UpdateUI")]
        //#endif
        private ICappedStat m_stat;
        [SerializeField]
        private bool m_trackMissingValue;

        public abstract float maxValue { set; }
        public abstract float currentValue { set; }

        public virtual void MonitorInfoOf(ICappedStat stat)
        {
            if (m_stat != null)
            {
                m_stat.ValueChanged -= OnValueChange;
                m_stat.MaxValueChanged -= OnMaxValueChange;
            }
            m_stat = stat;
            if (m_stat != null)
            {
                m_stat.ValueChanged += OnValueChange;
                m_stat.MaxValueChanged += OnMaxValueChange;
                Initialize(m_stat.maxValue, m_stat.currentValue);
            }
            else
            {
                maxValue = 0;
                currentValue = 0;
            }
        }

        protected virtual void Initialize(float maxValue, float currentValue)
        {
            this.maxValue = maxValue;
            if (m_trackMissingValue)
            {
                this.currentValue = maxValue - currentValue;
            }
            else
            {
                this.currentValue = currentValue;
            }
        }

        private void OnMaxValueChange(object sender, StatInfoEventArgs eventArgs) => maxValue = eventArgs.maxValue;

        private void OnValueChange(object sender, StatInfoEventArgs eventArgs)
        {
            if (m_trackMissingValue)
            {
                this.currentValue = eventArgs.maxValue - eventArgs.currentValue;
            }
            else
            {
                this.currentValue = eventArgs.currentValue;
            }
        }

        //#if UNITY_EDITOR
        private ICappedStat m_previous;

        protected virtual void Awake()
        {
            if (m_stat != null)
            {
                MonitorInfoOf(m_stat);
            }
        }

        private void UpdateUI()
        {
            if (m_previous != null)
            {
                m_previous.ValueChanged -= OnValueChange;
                m_previous.MaxValueChanged -= OnMaxValueChange;
            }

            m_stat.ValueChanged += OnValueChange;
            m_stat.MaxValueChanged += OnMaxValueChange;
            maxValue = m_stat.maxValue;
            currentValue = m_stat.currentValue;

            m_previous = m_stat;
        }
        //#endif
    }

}