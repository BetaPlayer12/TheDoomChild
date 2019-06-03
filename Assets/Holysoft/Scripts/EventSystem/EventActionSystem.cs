using Holysoft;
using Holysoft.Event.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Holysoft.Event
{

    namespace Systems
    {
        /// <summary>
        /// The Highlevel class that sends the events
        /// </summary>
        public class EventActionSystem : Singleton<EventActionSystem>
        {
            private static IEventActionMessenger m_messenger;

#if UNITY_EDITOR
            private static IEventActionLog m_logger;
            private static IEventActionConsole m_console;
            private static IEventActionDebug m_debug;
#endif

            protected override EventActionSystem currentInstance => this;

            private static IEventActionMessenger messenger
            {
                get
                {
                    if (m_messenger == null)
                    {
                        var instance = new EventActionSystem();
                    }
                    return m_messenger;
                }
            }

            public EventActionSystem() : base()
            {
                InitializedMessenger();
            }

#if UNITY_EDITOR
            public EventActionSystem(IEventActionLog logger, IEventActionConsole console)
            {
                InitializedMessenger();
                m_logger = logger;
                m_console = console;
            }

            public EventActionSystem(IEventActionDebug debug)
            {
                InitializedMessenger();
                m_debug = debug;
            }
#endif

            public static Type[] GetEventTypes()
            {
                var allClass = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                Type[] eventTypes = (from System.Type type in allClass where type.IsSubclassOf(typeof(IEventActionArgs)) select type).ToArray();
                List<Type> types = new List<Type>(eventTypes);
                types.Insert(0, typeof(IEventActionArgs));
                return types.ToArray(); ;
            }

            public static void Subscribe<T, U>(GameObject instance, EventAction<U> listener)
                where T : struct, IGlobalEventAction<U>
                where U : struct, IEventActionArgs
            {
                messenger.Subscribe<T, U>(listener);
#if UNITY_EDITOR
                if (m_debug == null)
                {
                    if (m_logger != null)
                    {
                        m_logger.LogAddedListener<T, U>(instance);
                    }
                }
                else
                {
                    m_debug.LogAddedListener<T, U>(instance);
                }
#endif
            }

            public static void Unsubscribe<T, U>(GameObject instance, EventAction<U> listener)
                where T : struct, IGlobalEventAction<U>
                where U : struct, IEventActionArgs
            {
                messenger.Unsubscribe<T, U>(listener);
#if UNITY_EDITOR
                if (m_debug == null)
                {
                    if (m_logger != null)
                    {
                        m_logger.LogRemovedListener<T, U>(instance);
                    }
                }
                else
                {
                    m_debug.LogRemovedListener<T, U>(instance);
                }
#endif
            }

            public static void Raise<T, U>(GameObject sender, U eventArgs)
                where T : struct, IGlobalEventAction<U>
                where U : struct, IEventActionArgs
            {
                messenger.Raise<T, U>(sender, eventArgs);
#if UNITY_EDITOR
                if (m_debug == null)
                {
                    if (m_console != null)
                    {
                        m_console.DisplayRaisedEvent<T, U>(sender);
                    }
                }
                else
                {
                    m_debug.DisplayRaisedEvent<T, U>(sender);
                }
#endif
            }

            private void InitializedMessenger() => m_messenger = new EventActionMessenger();
        }
    }
}