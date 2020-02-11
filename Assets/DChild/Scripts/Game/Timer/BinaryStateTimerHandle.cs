using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild
{
    [System.Serializable]
    public class BinaryStateTimerHandle : IEventTimerHandle
    {
        [SerializeField]
        private bool m_startAsActive;
        [SerializeField, MinValue(0), TabGroup("Active"),LabelText("Duration")]
        private float m_activeDuration;
        [SerializeField, TabGroup("Active"), HideReferenceObjectPicker, LabelText("Event")]
        private UnityEvent m_activeEvent = new UnityEvent();
        [SerializeField, MinValue(0), TabGroup("Inactive"), LabelText("Duration")]
        private float m_inactiveDuration;
        [SerializeField, TabGroup("Inactive"), HideReferenceObjectPicker, LabelText("Event")]
        private UnityEvent m_inactiveEvent = new UnityEvent();

        private bool m_isActive;
        private float m_time;

        public void Initialize()
        {
            if (m_startAsActive)
            {
                m_isActive = true;
                m_time = m_activeDuration;
                m_activeEvent?.Invoke();
            }
            else
            {
                m_isActive = false;
                m_time = m_inactiveDuration;
                m_inactiveEvent?.Invoke();
            }
        }

        public void Tick(float deltaTime)
        {
            m_time -= deltaTime;
            if (m_time <= 0)
            {
                if (m_isActive)
                {
                    m_isActive = false;
                    m_time = m_inactiveDuration;
                    m_inactiveEvent?.Invoke();
                }
                else
                {
                    m_isActive = true;
                    m_time = m_activeDuration;
                    m_activeEvent?.Invoke();
                }
            }
        }
    }
}