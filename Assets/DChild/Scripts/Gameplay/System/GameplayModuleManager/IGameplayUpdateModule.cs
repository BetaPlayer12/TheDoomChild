namespace DChild.Gameplay
{
    public interface IGameplayUpdateModule : IGameplayModuleManager
    {
        void UpdateModule(float deltaTime);
    }
}
