using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IDashController : ISubController
    {
        event EventAction<EventActionArgs> DashCall;
    }
}