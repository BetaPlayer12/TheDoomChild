using UnityEngine;
using System.Collections;

namespace DChild.Gameplay.Characters.Enemies
{
    public class WaitForHeartBeat : IEnumerator
    {
        bool m_wasFired = false;
        private HeartBeatHandle m_source;
        private int m_beatIndex;

        public WaitForHeartBeat(HeartBeatHandle source, int beatIndex = 0)
        {
            SafeSubscribe(source);
            m_beatIndex = beatIndex;
        }

        private void OnHeartBeat(int obj)
        {
            if (m_beatIndex == 0 || m_beatIndex == obj)
                m_wasFired = true;
        }

        private void SafeSubscribe(HeartBeatHandle source)
        {
            if (source == null)
            {
                // Break immediately if trackEntry is null.
                Debug.LogWarning("AnimationState was null. Coroutine will continue immediately.");
                m_wasFired = true;
            }
            else
            {
                m_source = source;
                m_source.OnBeat += OnHeartBeat;
            }
        }



        #region Reuse
        /// <summary>
        /// One optimization high-frequency YieldInstruction returns is to cache instances to minimize GC pressure. 
        /// Use NowWaitFor to reuse the same instance of WaitForSpineAnimationComplete.</summary>
        public WaitForHeartBeat NowWaitFor(HeartBeatHandle source, int beatIndex = 0)
        {
            SafeSubscribe(source);
            m_beatIndex = beatIndex;
            return this;
        }
        #endregion

        #region Enumerator
        public object Current => null;

        public bool MoveNext()
        {
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