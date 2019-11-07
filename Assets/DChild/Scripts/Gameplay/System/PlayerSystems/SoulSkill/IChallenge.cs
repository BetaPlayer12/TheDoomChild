using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public interface IChallenge
    {
        string message { get; } // use {x} to display soul skill name
        bool IsComplete();
        event EventAction<EventActionArgs> Completed;
        IChallenge CreateCopy();
    }
}