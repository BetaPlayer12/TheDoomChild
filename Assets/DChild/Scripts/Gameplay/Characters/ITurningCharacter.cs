using Holysoft.Event;

namespace DChild.Gameplay.Characters
{
    public struct FacingEventArgs : IEventActionArgs
    {
        public FacingEventArgs(HorizontalDirection currentFacingDirection) : this()
        {
            this.currentFacingDirection = currentFacingDirection;
        }

        public HorizontalDirection currentFacingDirection { get; }

    }

    public interface ITurningCharacter
    {
        event EventAction<FacingEventArgs> CharacterTurn;
    }
}
