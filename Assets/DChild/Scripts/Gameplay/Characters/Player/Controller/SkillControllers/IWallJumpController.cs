using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IWallJumpController : ISubController
    {
        event EventAction<EventActionArgs> WallJumpCall;
    }
}