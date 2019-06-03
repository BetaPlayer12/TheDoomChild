using Holysoft.Event;

namespace DChild
{
    public interface IValueChange
    {
        event EventAction<EventActionArgs> ValueChanged;
    }

    public interface IValueChange<T>
    {
        event EventAction<EventActionArgs<T>> ValueChanged;
    }
}