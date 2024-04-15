using DChild.Gameplay.Characters;
using Holysoft.Event;

namespace DChild.Gameplay
{
    public interface IController
    {
        event EventAction<EventActionArgs<bool>> ControllerStateChange; 
        void Enable();
        void Disable();
    }

    public interface IMoveHandle
    {
        void Move(HorizontalDirection direction);
        void Stop();
    }
}