namespace DChild.Gameplay.Systems.WorldComponents
{
    public interface IIsolatedTime
    {
        float deltaTime { get; }
        float fixedDeltaTime { get; }
    }
}