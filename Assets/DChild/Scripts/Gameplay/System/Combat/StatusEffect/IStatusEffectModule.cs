namespace DChild.Gameplay.Combat.StatusAilment
{
    public interface IStatusEffectModule
    {
        void Start(Character character);
        void Stop(Character character);
    }
}