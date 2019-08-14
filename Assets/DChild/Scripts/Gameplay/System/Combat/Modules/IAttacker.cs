using Holysoft.Event;

namespace DChild.Gameplay.Combat
{
    public interface IAttacker
    {
        event EventAction<CombatConclusionEventArgs> TargetDamaged;
    }
}