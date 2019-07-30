using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IWallStickController : ISubController
    {
        event EventAction<EventActionArgs> WallStickCall;
        event EventAction<EventActionArgs> WallSlideCall;
        event EventAction<EventActionArgs> WallStickCancel;
        event EventAction<ControllerEventArgs> AttempWallStickCall;
    }
}