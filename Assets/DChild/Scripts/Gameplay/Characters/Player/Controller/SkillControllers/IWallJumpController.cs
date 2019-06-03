using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IWallJumpController
    {
        event EventAction<EventActionArgs> WallJumpCall;
        event EventAction<ControllerEventArgs> UpdateCall;
    }
}