using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IFallController
    {
        event EventAction<EventActionArgs> FallUpdate;
        event EventAction<EventActionArgs> FallCall;
    }
}