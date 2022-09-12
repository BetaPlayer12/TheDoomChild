

using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [System.Serializable]
    public class IntervalIncapacitateHandle : IStatusEffectUpdatableModule
    {
        [SerializeField, MinValue(0.1)]
        private float m_interval;
        [SerializeField, MinValue(0.1)]
        private float m_durationPerInterval;

        private IController m_controller;
        private float m_currentTimer;
        private bool m_isInEffect;

        public IntervalIncapacitateHandle()
        {
            m_interval = 1f;
            m_durationPerInterval = 0.5f;
        }

        public IntervalIncapacitateHandle(float interval, float durationPerInterval)
        {
            m_interval = interval;
            m_durationPerInterval = durationPerInterval;
        }

        public void Initialize(Character character)
        {
            m_currentTimer = 0;
            m_isInEffect = false;
            m_controller = character.GetComponent<IController>();
        }

        public void Update(float delta)
        {
            m_currentTimer -= delta;
            if(m_currentTimer <= 0)
            {
                if (m_isInEffect)
                {
                    m_currentTimer = m_interval;
                    m_controller.Enable();
                }
                else
                {
                    m_currentTimer = m_durationPerInterval;
                    m_controller.Disable();
                }

                m_isInEffect = !m_isInEffect;
            }
        }
        public IStatusEffectUpdatableModule CreateCopy() => new IntervalIncapacitateHandle(m_interval, m_durationPerInterval);

#if UNITY_EDITOR
        [ShowInInspector, ReadOnly]
        private int m_totalElectricutionTimes;

        public void CalculateWithDuration(float duration)
        {
            m_totalElectricutionTimes = 0;

            duration -= m_durationPerInterval;
            m_totalElectricutionTimes++;
            do
            {
                duration -= m_interval + m_durationPerInterval;
                m_totalElectricutionTimes++;
            } while (duration > 0);
        }
#endif
    }
}