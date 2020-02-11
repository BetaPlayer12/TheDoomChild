using UnityEngine;
using Spine.Unity;
using UnityEngine.Events;
using Spine;
using System;

namespace DChild
{
    public class SpineDynamicEvent : MonoBehaviour, IHasSkeletonDataAsset
    {
        [System.Serializable]
        public class EventInfo
        {
            [SerializeField, SpineEvent]
            private string m_eventName;
            [SerializeField]
            private UnityEvent m_onEventCalled;

            public string eventName { get => m_eventName;}

            public void InvokeEvent()
            {
                m_onEventCalled?.Invoke();
            }
        }

        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField]
        private EventInfo[] m_eventInfo;
        SkeletonDataAsset IHasSkeletonDataAsset.SkeletonDataAsset => m_skeletonAnimation.SkeletonDataAsset;

        private static string m_cacheEvent;
        private static EventInfo m_cacheEventInfo;

        private void OnEvent(TrackEntry trackEntry, Spine.Event e)
        {
            m_cacheEvent = e.Data.Name;
            for (int i = 0; i < m_eventInfo.Length; i++)
            {
                m_cacheEventInfo = m_eventInfo[i];
                if (m_cacheEventInfo.eventName == m_cacheEvent)
                {
                    m_cacheEventInfo.InvokeEvent();
                    break;
                }
            }
        }
        private void Start()
        {
            m_skeletonAnimation.state.Event += OnEvent;
        }

    }
}