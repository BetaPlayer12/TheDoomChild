using UnityEngine;
using Spine.Unity;
using Spine;
using Sirenix.OdinInspector;
using System;

namespace DChild
{
    [RequireComponent(typeof(CallBackSounds))]
    [AddComponentMenu("DChild/Audio/Spine Sounds")]
    public class SpineSounds : MonoBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, InlineEditor]
        private SpineSoundData m_data;
        private CallBackSounds m_callback;

        private static string m_cacheEvent;
        private static SpineSoundData.EventInfo m_cacheEventInfo;
        private static string m_cacheAnimation;
        private static SpineSoundData.AnimationInfo m_cacheAnimationInfo;

        private void OnEvents(TrackEntry trackEntry, Spine.Event e)
        {
            m_cacheEvent = e.Data.Name;
            for (int i = 0; i < m_data.eventCount; i++)
            {
                m_cacheEventInfo = m_data.GetEventInfo(i);
                if (m_cacheEvent == m_cacheEventInfo.eventName)
                {
                    m_cacheEventInfo.PlaySound(m_callback);
                    break;
                }
            }
        }

        private void OnAnimationStart(TrackEntry trackEntry)
        {
            m_cacheAnimation = trackEntry.Animation.Name;
            for (int i = 0; i < m_data.animationCount; i++)
            {
                m_cacheAnimationInfo = m_data.GetAnimationInfo(i);
                if (m_cacheAnimation == m_cacheAnimationInfo.animationName)
                {
                    m_cacheAnimationInfo.StopSound(m_callback); //Gian Edit to fix the sounds that mutes when played again more than once
                    m_cacheAnimationInfo.PlaySound(m_callback);
                    break;
                }
            }
        }

        private void OnAnimationStop(TrackEntry trackEntry)
        {
            m_cacheAnimation = trackEntry.Animation.Name;
            for (int i = 0; i < m_data.animationCount; i++)
            {
                m_cacheAnimationInfo = m_data.GetAnimationInfo(i);
                if (m_cacheAnimation == m_cacheAnimationInfo.animationName)
                {
                    m_cacheAnimationInfo.StopSound(m_callback);
                    break;
                }
            }
        }

        private void Start()
        {
            m_callback = GetComponent<CallBackSounds>();
            m_skeletonAnimation.state.Event += OnEvents;
            m_skeletonAnimation.state.Start += OnAnimationStart;
            m_skeletonAnimation.state.Interrupt += OnAnimationStop;
        }


    }
}