using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{
    public class PlayerStatUI : SerializedMonoBehaviour
    {
        [SerializeField, BoxGroup("Health")]
        private ICappedStat m_healthStat;
        [SerializeField, BoxGroup("Health")]
        private Image m_healthGlow;

        private void HealthStatChange(object sender, StatInfoEventArgs eventArgs)
        {
            m_healthGlow.enabled = eventArgs.maxValue == eventArgs.currentValue;
        }

        private void Awake()
        {
            m_healthStat.MaxValueChanged += HealthStatChange;
            m_healthStat.ValueChanged += HealthStatChange;
            m_healthGlow.enabled = m_healthStat.maxValue == m_healthStat.currentValue;
        }
    }
}