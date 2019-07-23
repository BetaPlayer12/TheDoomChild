
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
        public bool loop;

        [ToggleGroup("useRootMotion")]
        public bool useRootMotion;
        [ToggleGroup("useRootMotion")]
        public bool useX;
        [ToggleGroup("useRootMotion")]
        public bool useY;

        public sealed override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (useRootMotion)
            {
                var rootMotion = animator.GetComponentInChildren<SpineRootMotion>();
                rootMotion.enabled = true;
                rootMotion.useX = useX;
                rootMotion.useY = useY;
            }
            ApplyAnimation(animator, stateInfo, layerIndex, animator.GetComponentInChildren<SkeletonAnimation>());
        }

        public sealed override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (useRootMotion)
            {
                var rootMotion = animator.GetComponentInChildren<SpineRootMotion>();
                rootMotion.enabled = false;
            }
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