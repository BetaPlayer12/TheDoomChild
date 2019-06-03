using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IFrozenController
    {
        event EventAction<ControllerEventArgs> CallFrozenHandler;
    }
}
