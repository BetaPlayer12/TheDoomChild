using System.Collections;

namespace Holysoft.Event
{
    public delegate void EventAction<EventArgs>(object sender, EventArgs eventArgs) where EventArgs : IEventActionArgs;
    public delegate Output EventAction<Output, EventArgs>(object sender, EventArgs eventArgs) where EventArgs : IEventActionArgs;
    public delegate void UnityEventAction<EventArgs>(object sender, EventArgs eventArgs) where EventArgs : UnityEngine.Object;
    public delegate Output UnityEventAction<Output, EventArgs>(object sender, EventArgs eventArgs) where EventArgs : UnityEngine.Object;

    public interface IEventActionArgs
    {
    }
}