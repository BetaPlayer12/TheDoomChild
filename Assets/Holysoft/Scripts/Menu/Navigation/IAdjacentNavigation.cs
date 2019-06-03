using Holysoft.Event;

namespace Holysoft.Menu
{
    public interface IAdjacentNavigation
    {
        void Previous();
        void Next();
    }

    public interface IAdjacentNavigationEvents
    {
        event EventAction<EventActionArgs> FirstItemReached;
        event EventAction<EventActionArgs> LastItemReached;
        event EventAction<EventActionArgs> NavigatingItem;
    }
}