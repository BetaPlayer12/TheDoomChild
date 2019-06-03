using DChild.Gameplay.Characters;
using Holysoft.Event;

namespace DChild.Gameplay.Combat
{
    public struct FlinchEventArgs : IEventActionArgs
    {
        public FlinchEventArgs(RelativeDirection damageSource) : this()
        {
            this.damageSource = damageSource;
        }

        public RelativeDirection damageSource { get; }
    }

    public interface IFlinchEvents
    {
        event EventAction<FlinchEventArgs> OnFlinch;
    }
}