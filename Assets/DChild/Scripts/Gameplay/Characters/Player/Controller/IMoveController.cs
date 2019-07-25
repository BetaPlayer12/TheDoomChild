using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IMoveController
    {
        event EventAction<ControllerEventArgs> MoveCall;
    }
}