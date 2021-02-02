using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class PlayerAvatar : MonoBehaviour
    {
        [System.Serializable]
        private class ModeShiftInfo
        {
            [SerializeField]
            private AnimationReferenceAsset m_startAnimation;
            [SerializeField]
            private AnimationReferenceAsset m_loopAnimation;
            [SerializeField]
            private AnimationReferenceAsset m_endAnimation;

            public AnimationReferenceAsset startAnimation => m_startAnimation;
            public AnimationReferenceAsset loopAnimation => m_loopAnimation;
            public AnimationReferenceAsset endAnimation => m_endAnimation;
        }

        [SerializeField]
        private AnimationReferenceAsset m_normalIdle;
        [SerializeField]
        private AnimationReferenceAsset m_flinch;

        [SerializeField]
        private ModeShiftInfo m_shadowMode;

        private ModeShiftInfo m_currentMode;
        private AnimationReferenceAsset m_currentIdleMode;
        private SkeletonGraphic m_animation;

        public void ExecuteFlinch()
        {
            ChangeToAnimation(m_flinch, m_currentIdleMode);
        }

        public void EndShadowMorph()
        {
            m_currentIdleMode = m_normalIdle;
            ChangeToAnimation(m_currentMode.endAnimation, m_currentIdleMode);
        }

        public void ExecuteShadowMorph()
        {
            ChangeToAnimation(m_shadowMode.startAnimation, m_shadowMode.loopAnimation);
            m_currentMode = m_shadowMode;
            m_currentIdleMode = m_currentMode.loopAnimation;
        }

        public void EndRage()
        {
        }

        public void ExecuteRage()
        {
        }

        public void EndArmor()
        {
        }

        public void ExecuteArmor()
        {
        }

        public void ExecuteIdle()
        {
            m_currentIdleMode = m_normalIdle;
            var idleTrack = m_animation.AnimationState.SetAnimation(0, m_currentIdleMode, true);
            idleTrack.MixDuration = 0f;
        }

        private void ChangeToAnimation(AnimationReferenceAsset start, AnimationReferenceAsset loop)
        {
            m_animation.AnimationState.SetAnimation(0, start, false);
            m_animation.AnimationState.AddAnimation(0, loop, true, 0);
        }
        private void Awake()
        {
            m_animation = GetComponent<SkeletonGraphic>();
        }
    }
}