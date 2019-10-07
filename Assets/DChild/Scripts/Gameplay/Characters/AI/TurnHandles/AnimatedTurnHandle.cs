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

        public void Execute(string turn, string idle)
        {
            m_animation.SetAnimation(0, turn, false);
            m_animation.AddAnimation(0, idle, true, 0);
            //m_animation.AddEmptyAnimation(0, 0, 0);
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
            var currentScale = m_character.facing == HorizontalDirection.Left ? Vector3.one : new Vector3(-1, 1, 1);
            m_character.transform.localScale = currentScale;
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