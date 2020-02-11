
using UnityEngine;
using Spine.Unity;
using Spine;

namespace DChild.StateMachine
{
    public class SpineAnimationExitState : SpineAnimationState
    {
        public string m_trigger;

        private Animator m_animator;
        private bool m_connected;

        protected override void ApplyAnimation(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, SkeletonAnimation skeletonAnimation)
        {
            base.ApplyAnimation(animator, stateInfo, layerIndex, skeletonAnimation);
            if (m_connected == false)
            {
                m_animator = animator;
                m_connected = true;
            }
        }

        protected override void ModifyTrack(TrackEntry track)
        {
            base.ModifyTrack(track);
            track.Complete += OnComplete;
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == m_animation.Animation.Name)
            {
                m_animator.SetTrigger(m_trigger);
                trackEntry.Complete -= OnComplete;
            }
        }
    }
}