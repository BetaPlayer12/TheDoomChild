using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface ILandController
    {
        event EventAction<EventActionArgs> LandCall;
    }
}