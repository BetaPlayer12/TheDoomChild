using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class IntervalEventHandle : MonoBehaviour
    {
        [SerializeField]
        private float m_startDelay;
        [SerializeField]
        private bool m_startAsActive;
        [SerializeField, TabGroup("Active")]
        private float m_activeDuration;
        [SerializeField, TabGroup("Inactive")]
        private float m_inactiveDuration;
        [SerializeField, TabGroup("Active")]
        private UnityEvent m_activeEvent;
        [SerializeField, TabGroup("Inactive")]
        private UnityEvent m_inactiveEvent;

        private bool m_isActivated = true;
        private bool m_isEnabled = true;
        private float m_startDelayTimer;
        private float m_activeTimer;
        private float m_inactiveTimer;

        public void Enable()
        {
            m_isEnabled = true;
        }

        public void Disable()
        {
            m_isEnabled = false;
        }

        private void Initialize()
        {
            enabled = true;
            m_startDelayTimer = m_startDelay;
            m_activeTimer = m_activeDuration;
            m_inactiveTimer = m_inactiveDuration;

            if (m_startDelayTimer == 0)
            {
                if (m_startAsActive == true)
                {
                    m_isActivated = true;
                    m_activeEvent?.Invoke();
                }
                else
                {
                    m_isActivated = false;
                    m_inactiveEvent?.Invoke();
                }
            }
            else
            {
                m_isActivated = false;
                m_inactiveEvent?.Invoke();
            }
        }

        public void Reset()
        {
            Initialize();
        }

        private void Start()
        {
            Initialize();
        }

        void Update()
        {
            if (m_isEnabled == true)
            {
                if (m_startDelayTimer > 0)
                {
                    m_startDelayTimer -= Time.deltaTime;

                    if (m_startDelayTimer <= 0)
                    {
                        if (m_startAsActive == true)
                        {
                            m_isActivated = true;
                            m_activeEvent?.Invoke();
                        }
                        else
                        {
                            m_isActivated = false;
                            m_inactiveEvent?.Invoke();
                        }
                    }
                }
                else if (m_startDelayTimer <= 0)
                {
                    if (m_isActivated == true)
                    {
                        m_activeTimer -= Time.deltaTime;

                        if (m_activeTimer <= 0)
                        {
                            m_isActivated = false;
                            m_inactiveEvent?.Invoke();

                            m_activeTimer = m_activeDuration;
                        }
                    }
                    else if (m_isActivated == false)
                    {
                        m_inactiveTimer -= Time.deltaTime;

                        if (m_inactiveTimer <= 0)
                        {
                            m_isActivated = true;
                            m_activeEvent?.Invoke();

                            m_inactiveTimer = m_inactiveDuration;
                        }
                    }
                }
            }
        }
    }
}