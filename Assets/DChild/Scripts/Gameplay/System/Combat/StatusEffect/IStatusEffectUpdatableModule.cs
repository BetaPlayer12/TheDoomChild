namespace DChild.Gameplay.Combat.StatusAilment
{

    public interface IStatusEffectUpdatableModule
    {
        void Initialize(Character character);
        void Update(float delta);
        IStatusEffectUpdatableModule CreateCopy();

#if UNITY_EDITOR
        void CalculateWithDuration(float duration);
#endif
    }
}