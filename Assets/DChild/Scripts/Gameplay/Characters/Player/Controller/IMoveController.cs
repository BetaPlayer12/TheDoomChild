using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IMoveController:ISubController
    {
        event EventAction<ControllerEventArgs> MoveCall;
    }
}