using System.Collections;
using System.Collections.Generic;

namespace Holysoft.Event
{
    public interface IEventListener
    {
#if UNITY_EDITOR
        void SubscribeToEvents();
        void UnsubscribeToEvents();
#endif
    }
}