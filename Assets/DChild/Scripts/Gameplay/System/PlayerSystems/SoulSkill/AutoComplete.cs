using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    public struct AutoComplete : IChallenge
    {
        public string message =>"Will Unlock {x} Once Aquired";

        public event EventAction<EventActionArgs> Completed;

        public IChallenge CreateCopy()
        {
            return new AutoComplete();
        }

        public bool IsComplete()
        {
            return true;
        }
    }
}