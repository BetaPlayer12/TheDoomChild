using Holysoft.Event;

namespace Holysoft.Menu
{
    public interface IAdjacentNavigation
    {
        event EventAction<EventActionArgs> FirstItemReached;
        event EventAction<EventActionArgs> LastItemReached;
        event EventAction<EventActionArgs> NavigatingItem;
        void Previous();
        void Next();
    }
}