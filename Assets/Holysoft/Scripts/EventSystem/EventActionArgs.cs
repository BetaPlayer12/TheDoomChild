using Holysoft.Event.Collections;
using UnityEngine;

namespace Holysoft.Event
{
    public class EventActionArgs : IEventActionArgs
    {
        private static EventActionArgs m_empty;
        public static EventActionArgs Empty
        {
            get
            {
                if (m_empty == null)
                {
                    m_empty = new EventActionArgs();
                }
                return m_empty;
            }
        }
    }

    public class EventActionArgs<T> : IEventActionArgs
    {
        public T info { get; private set; }
        public void Set(T info)
        {
            this.info = info;
        }
    }
}