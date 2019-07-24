
using UnityEngine;
using Spine.Unity;
using System;
using Sirenix.OdinInspector;
using System.Collections;
using Spine;
using Spine.Unity.Modules;

namespace DChild
{

    public class SpineAnimationState : StateMachineBehaviour
    {
        public AnimationReferenceAsset m_animation;
        public float m_speed = 1;
        public bool loop = true;

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ApplyAnimation(animator, stateInfo, layerIndex, animator.GetComponentInChildren<SkeletonAnimation>());
        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
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