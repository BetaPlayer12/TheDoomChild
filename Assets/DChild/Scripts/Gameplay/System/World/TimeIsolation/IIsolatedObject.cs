namespace DChild.Gameplay.Systems.WorldComponents
{

    public interface IIsolatedObject
    {
        void UpdateDeltaTime();
        float timeScale { get; }
        void SetTimeScale(float timeScale);
        void UpdateComponents();
        void UpdatePhysicsComponent();
        void CalculateActualVelocity();

#if UNITY_EDITOR
        int componentCount { get; }
#endif
    }
}