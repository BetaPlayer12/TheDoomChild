using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Collections
{
    [System.Serializable]
    public class CountdownTimer : ITimer
    {
        [SerializeField, MinValue(0f)]
        private float m_startTime = 0.1f;
        protected float m_currentTime = -1f;

        public CountdownTimer(float m_startTime)
        {
            this.m_startTime = m_startTime;
        }

        public event EventAction<EventActionArgs> CountdownEnd;

        public float time => m_currentTime;
        public float percentTime => m_currentTime / m_startTime;
        public bool hasEnded => m_currentTime <= -1f;
        public float startTime
        {
            get
            {
                return m_startTime;
            }
#if UNITY_EDITOR
            set
            {
                m_startTime = value;
            }
#endif
        }
        public void Reset() => m_currentTime = m_startTime;
        public void SetStartTime(float value) => m_startTime = value;

        public void EndTime(bool raiseEvent)
        {
            m_currentTime = -1f;
            if (raiseEvent)
            {
                CountdownEnd?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void Tick(float deltaTime)
        {
            if (m_currentTime >= 0)
            {
                m_currentTime -= deltaTime;
                if (m_currentTime <= 0)
                {
                    m_currentTime = -1f;
                    CountdownEnd?.Invoke(this, EventActionArgs.Empty);
                }
            }
        }
    }

}