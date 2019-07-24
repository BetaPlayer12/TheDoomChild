using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Holysoft.Gameplay.UI
{
    public abstract class CappedStatUI : SerializedMonoBehaviour
    {
        [OdinSerialize, OnValueChanged("UpdateUI")]
        private ICappedStat m_stat;

        protected abstract float maxValue { set; }
        protected abstract float currentValue { set; }

        public void MonitorInfoOf(ICappedStat stat)
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
                maxValue = m_stat.maxValue;
                currentValue = m_stat.currentValue;
            }
            else
            {
                maxValue = 0;
                currentValue = 0;
            }
        }

        private void OnMaxValueChange(object sender, StatInfoEventArgs eventArgs) => maxValue = eventArgs.maxValue;

        private void OnValueChange(object sender, StatInfoEventArgs eventArgs) => currentValue = eventArgs.currentValue;

#if UNITY_EDITOR
        private ICappedStat m_previous;

        private void Awake()
        {
            if (m_stat != null)
            {
                UpdateUI();
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
#endif
    }

}