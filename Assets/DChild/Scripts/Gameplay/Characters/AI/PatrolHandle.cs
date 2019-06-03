using Holysoft.Event;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Refactor.DChild.Gameplay.Character.AI
{
    public abstract class PatrolHandle : MonoBehaviour
    {
        public event EventAction<EventActionArgs> DestinationReached;

        public abstract void Execute(float speed);

        protected void CallDestinationReached() => DestinationReached?.Invoke(this, EventActionArgs.Empty);
    }
}