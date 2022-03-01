using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Visuals
{
    public class SpineAnimationStartOffset : MonoBehaviour
    {
        [SerializeField]
        private bool m_alwaysRandom;
        [SerializeField, MinValue(0), HideIf("m_alwaysRandom"), MaxValue("m_animationDuration")]
        private float m_startTime;

#if UNITY_EDITOR
        [Title("Editor Stuff")]
        [SerializeField, SpineAnimation, OnValueChanged("UpdateAnimationDuration")]
        private string m_animationReference;
        [SerializeField, ReadOnly]
        private float m_animationDuration;

        [Button]
        private void RandomizeStartTime()
        {
            m_startTime = Random.Range(0, m_animationDuration);
        }

        private void UpdateAnimationDuration()
        {
            var skeleton = GetComponent<SkeletonAnimation>().Skeleton;
            var animation = skeleton.Data.FindAnimation(m_animationReference);
            if (animation == null)
            {
                m_animationDuration = 0;
            }
            else
            {
                m_animationDuration = animation.Duration;
            }
        }
#endif

        // Start is called before the first frame update
        void Start()
        {
            if (m_alwaysRandom)
            {
                var skeletonAnimation = GetComponent<SkeletonAnimation>();
                var currentTrack = skeletonAnimation.state.GetCurrent(0);
                var skeleton = GetComponent<SkeletonAnimation>().Skeleton;
                var animation = skeleton.Data.FindAnimation(currentTrack.Animation.Name);
                currentTrack.TrackTime = Random.Range(0, animation.Duration);
            }
            else
            {
                GetComponent<SkeletonAnimation>().state.GetCurrent(0).TrackTime = m_startTime;
            }
        }
    }

}