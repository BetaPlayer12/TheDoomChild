using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IDoubleJumpController
    {
        event EventAction<EventActionArgs> DoubleJumpCall;
        event EventAction<EventActionArgs> DoubleJumpReset;
    }
}