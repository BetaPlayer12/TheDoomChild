using UnityEngine;

namespace Holysoft.Event
{
    public class EventListenersSubscriber : MonoBehaviour
    {
        private IEventListener[] m_listeners;

#if UNITY_EDITOR
        public void Subscribe()
        {
            m_listeners = GetComponentsInChildren<IEventListener>();
            for (int i = 0; i < m_listeners.Length; i++)
            {
                m_listeners[i].SubscribeToEvents();
            }
        }
#endif
    }

}