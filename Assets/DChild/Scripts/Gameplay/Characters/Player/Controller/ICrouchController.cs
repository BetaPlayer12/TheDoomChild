using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface ICrouchController : ISubController
    {
        event EventAction<ControllerEventArgs> CrouchCall;
        event EventAction<ControllerEventArgs> CrouchMoveCall;
    }
}