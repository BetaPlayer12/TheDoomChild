using Holysoft.Event;

namespace DChild.Gameplay.Combat
{
    public interface IAttackerEvents
    {
        event EventAction<CombatConclusionEventArgs> TargetDamaged;
    }
}