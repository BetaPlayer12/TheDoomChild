using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IGroundMoveController : ISubController
    {
        event EventAction<ControllerEventArgs> MoveCall;
    }
}