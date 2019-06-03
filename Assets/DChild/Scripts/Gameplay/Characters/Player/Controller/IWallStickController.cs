using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IWallStickController
    {
        event EventAction<EventActionArgs> WallStickCall;
        event EventAction<ControllerEventArgs> UpdateCall;
    }
}