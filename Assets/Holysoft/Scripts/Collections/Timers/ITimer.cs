namespace Holysoft.Collections
{
    public interface ITimer
    {
        float time { get; }
        void Reset();
        void Tick(float deltaTime);
    }
}