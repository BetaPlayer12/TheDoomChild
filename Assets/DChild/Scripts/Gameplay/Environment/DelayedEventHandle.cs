using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class DelayedEventHandle : MonoBehaviour
    {
        [SerializeField, MinValue(0)]
        private float m_delay;
        [SerializeField]
        private bool m_startOnAwake = false;
        [SerializeField]
        private UnityEvent m_event;

        private float m_delayTimer;
        private bool m_startTimer = false;
        private bool m_pauseTimer = false;

        public float currentDelayTimer => m_delayTimer;
        public float delayTime => m_delay;

        public void StartTimer()
        {
            m_startTimer = true;
            m_pauseTimer = false;
        }

        public void StopTimer()
        {
            m_startTimer = false;
        }

        public void PauseTimer()
        {
            m_startTimer = false;
            m_pauseTimer = true;
        }

        public void ResetTimer()
        {
            m_startTimer = false;
            m_delayTimer = m_delay;
        }

        public void SetTimerDelay(float timerValue)
        {
            m_delayTimer = timerValue;
        }

        private void Awake()
        {
            if (m_startOnAwake == true)
            {
                m_startTimer = true;
                m_pauseTimer = false;
            }
        }

        void Start()
        {
            m_delayTimer = m_delay;
        }

        void Update()
        {
            if (m_startTimer == true && m_pauseTimer == false)
            {
                if (m_delayTimer > 0)
                {
                    m_delayTimer -= GameplaySystem.time.deltaTime;

                    if (m_delayTimer <= 0)
                    {
                        m_event?.Invoke();
                    }
                }
            }
        }
    }
}