using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players
{
    public interface IPlayerModifer
    {
        event EventAction<ModifierChangeEventArgs> ModifierChange;
        float Get(PlayerModifier modifier);
    }
}