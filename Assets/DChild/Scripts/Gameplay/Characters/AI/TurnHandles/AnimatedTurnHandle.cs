using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Holysoft.Event;
using Spine;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class AnimatedTurnHandle : TurnHandle
    {
        [SerializeField]
        private SpineRootAnimation m_animation;
        [SerializeField]
        private bool m_useRootMotionX;
        [SerializeField]
        private bool m_useRootMotionY;

        private bool m_isTurning;
        private string m_turnAnimation;
        
        /// <summary>
        /// Use Only for Special Scenarios do not rely on this to replace Other Turn Handles
        /// </summary>
        public void ExecuteWithAnimationByPass()
        {
            TurnCharacter();
            Vector3 currentScale = GetFacingScale(m_character.facing);
            m_character.transform.localScale = currentScale;
        }

        public void Execute(string turn, string idle)
        {
            m_turnAnimation = turn;
            var turnAnimation = m_animation.SetAnimation(0, turn, false);
            m_animation.AddAnimation(0, idle, true, 0); //with idle
            //m_animation.AddEmptyAnimation(0, 0, 0);
            StartCoroutine(TurnRoutine(turnAnimation.Animation.Duration));
        }

        private IEnumerator TurnRoutine(float duration)
        {
            m_isTurning = true;
            m_animation.EnableRootMotion(m_useRootMotionX, m_useRootMotionY);
            m_animation.AnimationSet += OnAnimationSet;
            m_animation.animationState.Complete += OnComplete;
            m_animation.animationState.Interrupt += OnComplete;
            while (m_isTurning)
            {
                yield return null;
                duration -= GameplaySystem.time.deltaTime;
                if (duration <= 0 && m_animation.skeletonAnimation.enabled == false)
                {
                    break;
                }
            }
            m_animation.DisableRootMotion();
            m_animation.AnimationSet -= OnAnimationSet;
            m_animation.animationState.Complete -= OnComplete;
            m_animation.animationState.Interrupt -= OnComplete;
            TurnCharacter();
            Vector3 currentScale = GetFacingScale(m_character.facing);
            m_character.transform.localScale = currentScale;
            CallTurnDone(new FacingEventArgs(m_character.facing));
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == m_turnAnimation)
            {
                m_isTurning = false;
            }
        }

        private void OnAnimationSet(object sender, AnimationEventArgs eventArgs)
        {
            if (eventArgs.animationData.name != m_turnAnimation)
            {
                m_isTurning = false;
            }
        }
    }
}