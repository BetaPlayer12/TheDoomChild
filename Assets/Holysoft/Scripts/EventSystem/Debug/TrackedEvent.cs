using System;
using UnityEngine;

namespace Holysoft.Event.Collections
{
    public class NullTrackingDataException : Exception
    {
    }

    [System.Serializable]
    public class TrackedEvent
    {
        [SerializeField]
        private Type m_eventType;

        [SerializeField]
        private string m_eventName;

        [SerializeField]
        private bool m_isTrackingAll;

        [SerializeField]
        private bool m_isTrackingListeners;

        [SerializeField]
        private bool m_isTrackingRasiedEvent;

        public TrackedEvent(Type eventType)
        {
            m_eventType = eventType;
            m_eventName = m_eventType.ToString();
            m_isTrackingAll = true;
            m_isTrackingListeners = true;
            m_isTrackingRasiedEvent = true;
        }

        public TrackedEvent(TrackedEvent data)
        {
            m_eventType = data.eventType;
            m_eventName = m_eventType.ToString();
            m_isTrackingListeners = data.isTrackingListeners;
            m_isTrackingRasiedEvent = data.isTrackingRasiedEvent;
            m_isTrackingAll = m_isTrackingListeners && m_isTrackingRasiedEvent;
        }

        public Type eventType => m_eventType;

        public string eventName => m_eventName;

        public bool isTrackingListeners => m_isTrackingListeners;

        public bool isTrackingRasiedEvent => m_isTrackingRasiedEvent;

        public void Set(TrackedEvent data)
        {
            m_isTrackingListeners = data.isTrackingListeners;
            m_isTrackingRasiedEvent = data.isTrackingRasiedEvent;
            m_isTrackingAll = m_isTrackingListeners && m_isTrackingRasiedEvent;
        }
    }
}
