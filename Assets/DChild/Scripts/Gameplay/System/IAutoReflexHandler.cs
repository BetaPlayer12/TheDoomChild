using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IAutoReflexHandler
    {
        event EventAction<EventActionArgs> AutoReflexEnd;

        void StartAutoReflex();
        void StopAutoReflex();
    }
}