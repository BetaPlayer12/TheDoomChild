namespace DChild.Gameplay.Combat.StatusAilment
{
    public interface IStatusEffectModule
    {
        IStatusEffectModule GetInstance();

        void Start(Character character);
        void Stop(Character character);
    }
}