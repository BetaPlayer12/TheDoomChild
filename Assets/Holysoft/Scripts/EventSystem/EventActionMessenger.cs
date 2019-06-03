using Holysoft.Event.Collections;
using System;
using System.Collections.Generic;

namespace Holysoft.Event
{

    namespace Systems
    {
        public class EventActionMessenger : IEventActionMessenger
        {
            private readonly Dictionary<Type, Delegate> m_delegates;

            public EventActionMessenger()
            {
                m_delegates = new Dictionary<Type, Delegate>();
            }

            public void Raise<T, U>(object sender, U eventArgs)
                where T :  IGlobalEventAction<U>, new()
                where U :  IEventActionArgs
            {
                Delegate m_delegate;
                if (m_delegates.TryGetValue(typeof(T), out m_delegate)) //Checks if Event exists
                {
                    EventAction<U> callback = m_delegate as EventAction<U>;
                    if (callback != null)
                    {
                        this.Raise(callback, eventArgs);
                    }
                }
            }

            public void Subscribe<T, U>(EventAction<U> listener)
                where T :  IGlobalEventAction<U>, new()
                where U :  IEventActionArgs
            {
                Type type = typeof(T);
                var instance = new T();

                instance.Subscribe(listener);
                m_delegates[type] = instance.GetEventAction();
            }

            public void Unsubscribe<T, U>(EventAction<U> listener)
                where T :  IGlobalEventAction<U>, new()
                where U :  IEventActionArgs
            {
                Type type = typeof(T);
                var instance = new T();

                instance.Unsubscribe(listener);
                m_delegates[type] = instance.GetEventAction();
            }
        }
    }
}