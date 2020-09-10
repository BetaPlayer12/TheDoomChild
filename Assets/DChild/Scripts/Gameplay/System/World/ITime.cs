namespace DChild.Gameplay.Systems
{
    public interface ITime
    {
        float timeScale { get; }
        float deltaTime { get; }
        float fixedDeltaTime { get; }
    }

}