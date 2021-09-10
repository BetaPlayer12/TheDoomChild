namespace DChild.Gameplay
{
    public interface IGameplayFixedUpdateModule : IGameplayModuleManager
    {
        void FixedUpdateModule(float deltaTime);
    }
}
