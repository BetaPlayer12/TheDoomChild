using UnityEngine;

namespace Holysoft.Event
{
    namespace Collections
    {
#if UNITY_EDITOR

        public interface IEventActionDebug
        {
            void LogAddedListener<T, U>(GameObject listener) where T : struct, IGlobalEventAction<U> where U : struct, IEventActionArgs;

            void LogRemovedListener<T, U>(GameObject listener) where T : struct, IGlobalEventAction<U> where U : struct, IEventActionArgs;

            void DisplayRaisedEvent<T, U>(GameObject sender) where T : struct, IGlobalEventAction<U> where U : struct, IEventActionArgs;
        }
#endif
    }
}