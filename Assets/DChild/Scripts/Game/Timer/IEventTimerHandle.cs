namespace DChild
{
    public interface IEventTimerHandle
    {
        void Initialize();
        void Tick(float deltaTime);
    }
}