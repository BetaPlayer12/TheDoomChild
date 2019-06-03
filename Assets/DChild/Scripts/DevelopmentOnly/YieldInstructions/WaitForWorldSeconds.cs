using UnityEngine;
using System.Collections;
using DChild.Gameplay;
using DChild.Gameplay.Systems;

namespace DChild
{
    public class WaitForWorldSeconds : IEnumerator
    {
        bool m_wasFired = false;
        private float m_duration;
        private float m_timer;


        public WaitForWorldSeconds(float duration)
        {
            SafeSubscribe(duration);
        }

        private void HandleWait()
        {
            m_timer -= GameplaySystem.time.deltaTime;
            if (m_timer <= 0)
            {
                m_wasFired = true;
            }
        }

        private void SafeSubscribe(float duration)
        {
            if (duration <= 0)
            {
                m_wasFired = true;
            }
            else
            {
                m_duration = duration;
                m_timer = m_duration;
            }
        }

        #region Enumerator
        public object Current => null;

        public bool MoveNext()
        {
            HandleWait();
            if (m_wasFired)
            {
                ((IEnumerator)this).Reset();    // auto-reset for YieldInstruction reuse
                return false;
            }

            return true;
        }



        public void Reset() {
            m_timer = m_duration;
            m_wasFired = false; }
        #endregion
    }

}
