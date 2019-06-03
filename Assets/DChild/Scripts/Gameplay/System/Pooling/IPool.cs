namespace DChild.Gameplay.Pooling
{
    public interface IPool
    {
        void Update(float deltaTime);
        void Clear();
    }
}