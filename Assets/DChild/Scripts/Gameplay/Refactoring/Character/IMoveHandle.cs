using DChild.Gameplay.Characters;

namespace DChild.Gameplay
{
    public interface IMoveHandle
    {
        void Move(HorizontalDirection direction);
        void Stop();
    }
}