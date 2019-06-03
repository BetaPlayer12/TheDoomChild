using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IJumpController
    {
        event EventAction<EventActionArgs> JumpCall;
    }
}