using UnityEngine;

namespace Holysoft.Event
{
    namespace Collections
    {
#if UNITY_EDITOR
        /// <summary>
        /// Stores information about listeners
        /// </summary>
        public interface IEventActionLog
        {
            void LogAddedListener<T, U>(GameObject listener) where T : struct, IGlobalEventAction<U> where U : struct, IEventActionArgs;

            void LogRemovedListener<T, U>(GameObject listener) where T : struct, IGlobalEventAction<U> where U : struct, IEventActionArgs;

            void ClearListeners();
        }
#endif
    }
}