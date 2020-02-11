using System;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public class PatienceHandle
    {
        private float m_duration;
        private float m_currentTime;
        private bool m_hasStarted;
        private Action OnEndOfPatience;

        public void Start()
        {
            m_currentTime = m_duration;
            m_hasStarted = true;
        }

        public void End()
        {
            m_hasStarted = false;
        }


        public void Update(float deltaTime)
        {
            if (m_hasStarted)
            {
                m_currentTime -= deltaTime;
                if (m_currentTime < 0)
                {
                    m_hasStarted = false;
                    OnEndOfPatience?.Invoke();
                } 
            }
        }

        public PatienceHandle(float duration, Action onEndOfPatience)
        {
            m_duration = duration;
            OnEndOfPatience = onEndOfPatience;
        }
    }
}