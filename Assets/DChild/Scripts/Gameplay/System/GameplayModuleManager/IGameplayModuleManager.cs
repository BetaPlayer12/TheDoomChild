namespace DChild.Gameplay
{
    public interface IGameplayModuleManager
    {
        string name { get; }
        void SetInstance(IGameplayModuleManager instance);
    }
}
