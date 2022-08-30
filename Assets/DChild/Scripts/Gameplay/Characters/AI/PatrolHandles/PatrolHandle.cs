using DChild.Gameplay.Characters;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public abstract class PatrolHandle : MonoBehaviour
    {
        public struct CharacterInfo
        {
            public CharacterInfo(Vector2 position, HorizontalDirection currentFacing) : this()
            {
                this.position = position;
                this.currentFacing = currentFacing;
            }

            public Vector2 position { get; }
            public HorizontalDirection currentFacing { get; }
        }
        public abstract Vector2 currentDestination { get; }
        public event EventAction<EventActionArgs> TurnRequest;
        public event EventAction<EventActionArgs> DestinationReached;


        public abstract void Patrol(MovementHandle2D movement, float speed, CharacterInfo characterInfo);
        public abstract void Patrol(PathFinderAgent agent, float speed, CharacterInfo characterInfo);

        protected void CallTurnRequest() => TurnRequest?.Invoke(this, EventActionArgs.Empty);
        protected void CallDestinationReached() => DestinationReached?.Invoke(this, EventActionArgs.Empty);
        protected HorizontalDirection GetProposedFacing(Vector2 characterPosition, Vector2 destination) => destination.x < characterPosition.x ? HorizontalDirection.Left : HorizontalDirection.Right;
    }
}