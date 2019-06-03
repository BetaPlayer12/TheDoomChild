using UnityEngine;

namespace Holysoft.Event
{
    namespace Collections
    {
#if UNITY_EDITOR
        public interface IEventActionConsole
        {
            void DisplayAddedListeners();

            void DisplayRemovedListeners();

            void DisplayRaisedEvent<T, U>(GameObject sender) where T : struct, IGlobalEventAction<U> where U : struct, IEventActionArgs;
        }
#endif
    }
}