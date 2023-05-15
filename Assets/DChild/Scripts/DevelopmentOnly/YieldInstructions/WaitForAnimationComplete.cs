using UnityEngine;
using System.Collections;
using Spine;
using System;
using DChild.Gameplay.Characters.AI;

namespace DChild
{
    public class WaitForAnimationComplete : IEnumerator
    {
        bool m_wasFired = false;
        private string m_animationName;
        private Spine.AnimationState m_state;

        public WaitForAnimationComplete(Spine.AnimationState state, string animationName)
        {
            SafeSubscribe(state);
            m_animationName = animationName;
        }

        public WaitForAnimationComplete(Spine.AnimationState state, IAIAnimationInfo animationInfo)
        {
            SafeSubscribe(state);
            m_animationName = animationInfo.animation;
        }

        public WaitForAnimationComplete()
        {
        }

        private void HandleComplete(TrackEntry trackEntry)
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
                m_state = state;
                m_state.Complete += HandleComplete;
            }
        }

        #region Reuse
        /// <summary>
        /// One optimization high-frequency YieldInstruction returns is to cache instances to minimize GC pressure. 
        /// Use NowWaitFor to reuse the same instance of WaitForSpineAnimationComplete.</summary>
        public WaitForAnimationComplete NowWaitFor(Spine.AnimationState state, string animationName)
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
