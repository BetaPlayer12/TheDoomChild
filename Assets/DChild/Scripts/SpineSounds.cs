using UnityEngine;
using Spine.Unity;
using Spine;
using DarkTonic.MasterAudio;

namespace DChild
{
    public class SpineSounds : MonoBehaviour
    {
        [System.Serializable]
        private class EventInfo
        {
            [SerializeField, SpineEvent]
            private string m_eventName;

            [SerializeField,SoundGroup]
            private string m_soundToPlay;

            public string eventName { get => m_eventName; }

            public void PlaySound(Transform transform) { }
        }

        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField]
        private EventInfo[] m_eventInfo;

        private string m_cacheEvent;
        private EventInfo m_cacheInfo;

        private void OnEvents(TrackEntry trackEntry, Spine.Event e)
        {
            m_cacheEvent = e.Data.Name;
            for (int i = 0; i < m_eventInfo.Length; i++)
            {
                m_cacheInfo = m_eventInfo[i];
                if (m_cacheEvent == m_cacheInfo.eventName)
                {
                    m_cacheInfo.PlaySound(transform);
                    break;
                }
            }
        }

        private void Start()
        {
            m_skeletonAnimation.state.Event += OnEvents;
        }
    }
}