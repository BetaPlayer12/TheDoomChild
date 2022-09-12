using DChild.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    public class TimedCollider2D : MonoBehaviour
    {
        [SerializeField, MinValue(0)]
        private float m_activeDelay;
        [SerializeField, MinValue(0)]
        private float m_deactiveDelay;

        private Collider2D m_collider;
        private bool m_isActive;
        private float m_timer;

        public void SetActive(bool active)
        {
            if (active)
            {
                if (m_isActive == false)
                {
                    m_isActive = true;
                    m_timer = m_activeDelay;
                    if (m_activeDelay == 0)
                    {
                        m_collider.enabled = true;
                        enabled = false;
                    }
                    else
                    {
                        enabled = true;
                    }
                }
            }
            else
            {
                if (m_isActive)
                {
                    m_isActive = false;
                    m_timer = m_deactiveDelay;
                    if (m_deactiveDelay == 0)
                    {
                        m_collider.enabled = false;
                        enabled = false;
                    }
                    else
                    {
                        enabled = true;
                    }
                }
            }
        }

        private void Awake()
        {
            m_collider = GetComponent<Collider2D>();
            m_collider.enabled = false;
            enabled = false;
            m_isActive = false;
        }

        private void Update()
        {
            m_timer -= GameplaySystem.time.deltaTime;
            if (m_timer <= 0)
            {
                m_collider.enabled = m_isActive;
                enabled = false;
            }
        }
    }

}