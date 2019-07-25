using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IDashController
    {
        event EventAction<EventActionArgs> DashCall;
    }
}