using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IDoubleJumpController : ISubController
    {
        event EventAction<EventActionArgs> DoubleJumpCall;
    }
}