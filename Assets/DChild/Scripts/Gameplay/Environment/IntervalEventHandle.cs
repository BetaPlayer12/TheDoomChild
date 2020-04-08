using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class IntervalEventHandle : MonoBehaviour
    {
        [SerializeField]
        private float m_startDelay;
        [SerializeField]
        private float m_activateDelay;
        [SerializeField]
        private float m_deactivateDelay;
        [SerializeField, TabGroup("Activate")]
        private UnityEvent m_activateEvent;
        [SerializeField, TabGroup("Deactivate")]
        private UnityEvent m_deactivateEvent;

        private bool m_isActivated = true;
        private bool m_isEnabled = true;
        private float m_startDelayTimer;
        private float m_activateTimer;
        private float m_deactivateTimer;

        public void Enable()
        {
            m_isEnabled = true;
        }

        public void Disable()
        {
            m_isEnabled = false;
        }

        private void Start()
        {
            m_startDelayTimer = m_startDelay;
            m_activateTimer = m_activateDelay;
            m_deactivateTimer = m_deactivateDelay;
        }

        void Update()
        {
            if (m_isEnabled == true)
            {
                if(m_startDelayTimer > 0)
                {
                    m_startDelayTimer -= Time.deltaTime;
                }
                else if(m_startDelayTimer <= 0)
                {
                    if (m_isActivated == true)
                    {
                        m_deactivateTimer -= Time.deltaTime;

                        if (m_deactivateTimer <= 0)
                        {
                            m_isActivated = false;
                            m_deactivateEvent?.Invoke();

                            m_deactivateTimer = m_deactivateDelay;
                        }
                    }
                    else if (m_isActivated == false)
                    {
                        m_activateTimer -= Time.deltaTime;

                        if (m_activateTimer <= 0)
                        {
                            m_isActivated = true;
                            m_activateEvent?.Invoke();

                            m_activateTimer = m_activateDelay;
                        }
                    }
                }
            }
        }
    }
}