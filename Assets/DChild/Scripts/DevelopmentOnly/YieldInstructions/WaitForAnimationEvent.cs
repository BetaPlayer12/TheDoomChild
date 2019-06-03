using UnityEngine;
using System.Collections;
using Spine;

namespace DChild
{
    public class WaitForAnimationEvent : IEnumerator
    {
        bool m_wasFired = false;
        private string m_eventName;

        public WaitForAnimationEvent(Spine.AnimationState state, string eventName)
        {
            SafeSubscribe(state);
            m_eventName = eventName;
        }

        private void SafeSubscribe(Spine.AnimationState state)
        {
            if (state == null)
            {
                // Break immediately if trackEntry is null.
                Debug.LogWarning("AnimationState was null. Coroutine will continue immediately.");
                m_wasFired = true;
            }
            else
            {
                state.Event += HandleEvent;
            }
        }

        private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if(e.Data.Name == m_eventName)
            {
                m_wasFired = true;
            }
        }

        #region Reuse
        /// <summary>
        /// One optimization high-frequency YieldInstruction returns is to cache instances to minimize GC pressure. 
        /// Use NowWaitFor to reuse the same instance of WaitForSpineAnimationComplete.</summary>
        public WaitForAnimationEvent NowWaitFor(Spine.AnimationState state, string eventName)
        {
            SafeSubscribe(state);
            m_eventName = eventName;
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
