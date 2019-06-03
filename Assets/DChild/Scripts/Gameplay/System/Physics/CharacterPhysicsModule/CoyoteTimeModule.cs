using UnityEngine;

namespace DChild.Gameplay.Physics
{
    [System.Serializable]
    public class CoyoteTimeModule
    {
        [SerializeField]
        private float m_duration;
        private float m_timer;
        private bool m_isActive;
        private bool m_isOver;

        public bool isAvailable => m_isOver == false;

        public void Start()
        {
            if(m_isActive == false && m_isOver == false)
            {
                m_isActive = true;
                m_timer = m_duration;
            }
        }

        public void Update(float deltaTime)
        {
            if (m_isActive)
            {
                m_timer -= deltaTime;
                if (m_timer <= 0)
                {
                    m_isActive = false;
                    m_isOver = true;
                }
            }
        }

        public void Reset()
        {
            m_isActive = false;
            m_isOver = false;
        }

        public void Stop()
        {
            m_isActive = false;
            m_isOver = true;
        }
    }
}
