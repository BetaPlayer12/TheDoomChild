namespace DChild.Gameplay
{
    public interface IGameplayLateUpdateModule: IGameplayModuleManager
    {
        void LateUpdateModule(float deltaTime);
    }
}
