using UnityEngine;
using System.Collections;
using Spine;
using DChild.Gameplay;

namespace DChild
{
    public class WaitForAnimationStart : IEnumerator
    {
        bool m_wasFired = false;
        private string m_animationName;

        public WaitForAnimationStart(Spine.AnimationState state, string animationName)
        {
            SafeSubscribe(state);
            m_animationName = animationName;
        }

        private void HandleStart(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == m_animationName)
            {
                m_wasFired = true;
            }
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
                state.Start += HandleStart;
            }
        }

        #region Reuse
        /// <summary>
        /// One optimization high-frequency YieldInstruction returns is to cache instances to minimize GC pressure. 
        /// Use NowWaitFor to reuse the same instance of WaitForSpineAnimationComplete.</summary>
        public WaitForAnimationStart NowWaitFor(Spine.AnimationState state, string animationName)
        {
            SafeSubscribe(state);
            m_animationName = animationName;
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
