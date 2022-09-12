using Boo.Lang;
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
        [SerializeField]
        private ModeShiftInfo m_rageMode;

        private List<ModeShiftInfo> m_activeModes;
        private ModeShiftInfo m_currentMode;
        private AnimationReferenceAsset m_currentIdleMode;
        private SkeletonGraphic m_animation;

        public void ExecuteFlinch()
        {
            ChangeToAnimation(m_flinch, m_currentIdleMode);
        }

        public void EndShadowMorph()
        {
            EndMode(m_shadowMode);
            //m_currentIdleMode = m_normalIdle;
            //ChangeToAnimation(m_currentMode.endAnimation, m_currentIdleMode);
        }

        public void ExecuteShadowMorph(bool executeAnimation = true)
        {
            if (executeAnimation)
            {
                ChangeToAnimation(m_shadowMode.startAnimation, m_shadowMode.loopAnimation);
                m_currentMode = m_shadowMode;
                m_currentIdleMode = m_currentMode.loopAnimation;
            }
            RecordToList(m_shadowMode);
        }

        public void EndRage()
        {
            EndMode(m_rageMode);
            //m_currentIdleMode = m_normalIdle;
            //ChangeToAnimation(m_currentMode.endAnimation, m_currentIdleMode);
        }

        public void ExecuteRage(bool executeAnimation = true)
        {
            if (executeAnimation)
            {
                ChangeToAnimation(m_rageMode.startAnimation, m_rageMode.loopAnimation);
                m_currentMode = m_rageMode;
                m_currentIdleMode = m_currentMode.loopAnimation;
            }
            RecordToList(m_rageMode);
        }

        public void EndArmor()
        {
        }

        public void ExecuteArmor()
        {
        }

        private void EndMode(ModeShiftInfo mode)
        {
            if (m_currentMode == mode)
            {
                m_activeModes.RemoveAt(m_activeModes.Count - 1);
                if (m_activeModes.Count == 0)
                {
                    m_currentIdleMode = m_normalIdle;
                    ChangeToAnimation(m_currentMode.endAnimation, m_currentIdleMode);
                    m_currentMode = null;
                }
                else
                {
                    var nextMode = m_activeModes[m_activeModes.Count - 1];
                    ChangeToAnimation(m_currentMode.endAnimation, nextMode.loopAnimation);
                    m_currentMode = nextMode;
                    m_currentIdleMode = nextMode.loopAnimation;
                }
            }
            else
            {
                m_activeModes.Remove(mode);
            }
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

        private void RecordToList(ModeShiftInfo mode)
        {
            if (m_activeModes.Contains(mode))
            {
                m_activeModes.Remove(mode);
            }
                m_activeModes.Add(mode);
        }

        private void Awake()
        {
            m_animation = GetComponent<SkeletonGraphic>();
            m_activeModes = new List<ModeShiftInfo>();
        }
    }
}