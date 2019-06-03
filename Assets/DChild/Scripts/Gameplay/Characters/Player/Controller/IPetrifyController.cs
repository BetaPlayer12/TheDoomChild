using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IPetrifyController
    {
        event EventAction<ControllerEventArgs> CallPetrifyHandler;
    }
}
