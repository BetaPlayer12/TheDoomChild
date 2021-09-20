using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class ParticleFXCollider : MonoBehaviour, ParticleFXComponent
    {
        [SerializeField, MinValue(0)]
        private float m_enableDelay;
        [SerializeField, MinValue(0)]
        private float m_enableDuration;

        private Collider2D m_collider;
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private float m_timer;
        private bool m_isEnableDelayed;

        public void Initialize()
        {
            m_collider = GetComponent<Collider2D>();
            enabled = false;
            m_collider.enabled = false;
        }

        public void Reset()
        {
            enabled = true;
            m_isEnableDelayed = m_enableDelay > 0;
            if (m_isEnableDelayed)
            {
                m_collider.enabled = false;
                m_timer = m_enableDelay;
            }
            else
            {
                m_collider.enabled = true;
                m_timer = m_enableDuration;
            }
        }

        public void SetActive(bool isActive)
        {
            enabled = isActive;
        }

        public void Stop()
        {
            enabled = false;
            m_collider.enabled = false;
        }

        private void Update()
        {
            if (m_collider.isActiveAndEnabled || m_isEnableDelayed)
            {
                m_timer -= GameplaySystem.time.deltaTime;
                if (m_timer <= 0)
                {
                    if (m_isEnableDelayed)
                    {
                        m_collider.enabled = true;
                        m_timer = m_enableDuration;
                    }
                    else
                    {
                        m_collider.enabled = false;
                    }
                }
            }
        }
    }
}