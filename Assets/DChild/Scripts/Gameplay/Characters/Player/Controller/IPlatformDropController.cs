using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IPlatformDropController
    {
        event EventAction<ControllerEventArgs> PlatformDropCall;
    }
}