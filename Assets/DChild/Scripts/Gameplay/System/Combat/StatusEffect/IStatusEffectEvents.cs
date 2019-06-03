using Holysoft.Event;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public interface IStatusEffectEvents
    {
        event EventAction<StatusEffectEventArgs> EffectStart;
        event EventAction<StatusEffectEventArgs> EffectEnd;
    }
}