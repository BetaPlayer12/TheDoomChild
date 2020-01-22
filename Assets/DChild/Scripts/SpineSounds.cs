using UnityEngine;
using Spine.Unity;
using Spine;
using DarkTonic.MasterAudio;
using System;

namespace DChild
{
    [AddComponentMenu("DChild/Audio/Spine Sounds")]
    public class SpineSounds : MonoBehaviour, IHasSkeletonDataAsset
    {
        [System.Serializable]
        private class EventInfo
        {
            [SerializeField, SpineEvent]
            private string m_eventName;

            [SerializeField,SoundGroup]
            private string m_soundToPlay;

            public string eventName { get => m_eventName; }

            public void PlaySound(Transform transform) => MasterAudio.PlaySound3DAtTransformAndForget(m_soundToPlay, transform);
        }

        [System.Serializable]
        private class AnimationInfo
        {
            [SerializeField, SpineAnimation]
            private string m_animationName;

            [SerializeField, SoundGroup]
            private string m_soundToPlay;

            public string animationName { get => m_animationName; }

            public void PlaySound(Transform transform)
            {
                MasterAudio.PlaySound3DAtTransformAndForget(m_soundToPlay, transform);
            }
        }

        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField]
        private EventInfo[] m_eventInfo;
        [SerializeField]
        private AnimationInfo[] m_animationStartInfo;

        private static string m_cacheEvent;
        private static EventInfo m_cacheEventInfo;
        private static string m_cacheAnimation;
        private static AnimationInfo m_cacheAnimationInfo;

        SkeletonDataAsset IHasSkeletonDataAsset.SkeletonDataAsset => m_skeletonAnimation.SkeletonDataAsset;

        private void OnEvents(TrackEntry trackEntry, Spine.Event e)
        {
            m_cacheEvent = e.Data.Name;
            for (int i = 0; i < m_eventInfo.Length; i++)
            {
                m_cacheEventInfo = m_eventInfo[i];
                if (m_cacheEvent == m_cacheEventInfo.eventName)
                {
                    m_cacheEventInfo.PlaySound(transform);
                    break;
                }
            }
        }

        private void OnAnimationStart(TrackEntry trackEntry)
        {
            m_cacheAnimation = trackEntry.Animation.Name;
            for (int i = 0; i < m_animationStartInfo.Length; i++)
            {
                m_cacheAnimationInfo = m_animationStartInfo[i];
                if(m_cacheAnimation == m_cacheAnimationInfo.animationName)
                {
                    m_cacheAnimationInfo.PlaySound(transform);
                    break;
                }
            }
        }

        private void Start()
        {
            m_skeletonAnimation.state.Event += OnEvents;
            m_skeletonAnimation.state.Start += OnAnimationStart;
        }

     
    }
}