﻿
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay
{
    public abstract class PathFinderAgent : MonoBehaviour
    {
        public event EventAction<EventActionArgs> OnDestinationReached; 

        public abstract Vector2 segmentDestination { get; }
        public abstract bool hasPath { get; }

        public abstract void SetDestination(Vector2 position);

        public abstract void Move(float speed);

        public abstract void MoveTowardsForced(Vector2 direction, float speed);

        public abstract void Stop();

        protected void SendDestionationReachedEvent() => OnDestinationReached?.Invoke(this, EventActionArgs.Empty);
    }
}