using DChild.Gameplay.Characters;

namespace DChild.Gameplay
{
    public interface IController
    {
        void Enable();
        void Disable();
    }

    public interface IMoveHandle
    {
        void Move(HorizontalDirection direction);
        void Stop();
    }
}