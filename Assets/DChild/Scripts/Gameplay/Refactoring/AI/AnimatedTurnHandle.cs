using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Holysoft.Event;
using Spine;
using System;
using System.Collections;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters
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

        public void Execute(string animation, out bool waitForBehaviour)
        {
            waitForBehaviour = false;
            m_animation.SetAnimation(0, animation, false);
            StartCoroutine(TurnRoutine());
        }

        private IEnumerator TurnRoutine()
        {
            m_isTurning = true;
            m_animation.EnableRootMotion(m_useRootMotionX, m_useRootMotionY);
            m_animation.AnimationSet += OnAnimationSet;
            m_animation.animationState.Complete += OnComplete;
            while (m_isTurning)
            {
                yield return null;
            }
            m_animation.DisableRootMotion();
            m_animation.AnimationSet -= OnAnimationSet;
            m_animation.animationState.Complete -= OnComplete;
            TurnCharacter();
            CallTurnDone(new FacingEventArgs(m_character.facing));
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            m_isTurning = false;
        }

        private void OnAnimationSet(object sender, AnimationEventArgs eventArgs)
        {
            m_isTurning = false;
        }
    }
}