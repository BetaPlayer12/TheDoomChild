
using UnityEngine;
using Spine.Unity;
using System;
using System.Collections;
using Spine;

namespace DChild.StateMachine
{
    public class SpineAnimationState : StateMachineBehaviour
    {
        public AnimationReferenceAsset m_animation;
        public float m_speed = 1;
        public bool loop;

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ApplyAnimation(animator, stateInfo, layerIndex, animator.GetComponentInChildren<SkeletonAnimation>());
        }

        protected virtual void ApplyAnimation(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, SkeletonAnimation skeletonAnimation)
        {
            var track = animator.GetComponentInChildren<SkeletonAnimation>().state.SetAnimation(layerIndex, m_animation, loop);
            ModifyTrack(track);
        }

        protected virtual void ModifyTrack(TrackEntry track)
        {
            track.TimeScale = m_speed;
        }
    }
}