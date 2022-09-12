using Holysoft.Event;

namespace DChildDebug.Window
{
    public interface ITrackableValue
    {
        float value { get; }
        event EventAction<EventActionArgs> ValueChange;
    }

}