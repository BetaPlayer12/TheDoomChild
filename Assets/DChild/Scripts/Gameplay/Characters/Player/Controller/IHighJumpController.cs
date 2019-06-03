using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IHighJumpController 
    {
        event EventAction<ControllerEventArgs> HighJumpCall;
    }
}