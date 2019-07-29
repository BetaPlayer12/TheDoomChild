using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IJumpController : ISubController
    {
        event EventAction<ControllerEventArgs> JumpCall;
    }
}