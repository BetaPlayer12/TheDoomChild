using UnityEngine;
using System.Collections;
using DChild.Gameplay;
using DChild.Gameplay.Systems.WorldComponents;

namespace DChild
{
    public class WaitForIsolatedSeconds : IEnumerator
    {
        bool m_wasFired = false;
        private IIsolatedTime m_isolatedTime;
        private float m_timer;


        public WaitForIsolatedSeconds(float duration, IIsolatedTime isolatedTime)
        {
            SafeSubscribe(duration, isolatedTime);    
        }

        private void HandleWait()
        {
            m_timer -= m_isolatedTime.deltaTime;
            if (m_timer <= 0)
            {
                m_wasFired = true;
            }
        }

        private void SafeSubscribe(float duration, IIsolatedTime isolatedTime)
        {
            if (duration <= 0 || isolatedTime == null)
            {
                // Break immediately if trackEntry is null.
                Debug.LogWarning("AnimationState was null. Coroutine will continue immediately.");
                m_wasFired = true;
            }
            else
            {
                m_isolatedTime = isolatedTime;
                m_timer = duration;
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

     

        public void Reset() { m_wasFired = false; }
        #endregion
    }

}
