namespace DChild.Gameplay.Combat.StatusAilment
{
    public struct Incapacitate : IStatusEffectModule
    {
        public IStatusEffectModule GetInstance() => this;

        public void Start(Character character)
        {
            character.GetComponent<IController>()?.Disable();
        }

        public void Stop(Character character)
        {
            character.GetComponent<IController>()?.Enable();
        }
    }
}