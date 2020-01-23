using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild
{
    [System.Serializable]
    public class FixedIntervalTimerHandle : IEventTimerHandle
    {
        [SerializeField, MinValue(0)]
        private float m_interval;
        [SerializeField, HideReferenceObjectPicker]
        private UnityEvent m_event = new UnityEvent();

        private float m_time;
        public void Initialize()
        {
            m_time = m_interval;
        }

        public void Tick(float deltaTime)
        {
            m_time -= deltaTime;
            if (m_time <= 0)
            {
                m_event?.Invoke();
                m_time = m_interval;
            }
        }
    }
}