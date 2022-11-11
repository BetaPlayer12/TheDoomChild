using DChild.Temp;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Trohpies.Modules
{
    public class EventBaseModule : ITrophyModule
    {
        [SerializeField]
        private string m_eventToListen;
        private bool m_isComplete;

        public event EventAction<EventActionArgs> Complete;

        public EventBaseModule()
        {
            m_eventToListen = "";
            m_isComplete = false;
        }

        public EventBaseModule(string eventToListen)
        {
            m_eventToListen = eventToListen;
            m_isComplete = false;
        }

        public ITrophyModule CreateCopy()
        {
            return new EventBaseModule(m_eventToListen);
        }

        public void Initialize()
        {
            m_isComplete = false;
            GameEventMessage.AddListener<GameEventMessage>(OnEventCalled);
        }

        private void OnEventCalled(GameEventMessage message)
        {
            if (m_isComplete == false && message.EventName == m_eventToListen)
            {
                m_isComplete = true;
                Complete?.Invoke(this, EventActionArgs.Empty);
                GameEventMessage.RemoveListener<GameEventMessage>(OnEventCalled);
            }
        }

        public bool isComplete()
        {
            return m_isComplete;
        }
    }
}