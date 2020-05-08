using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public struct HitDirectionEventArgs : IEventActionArgs
    {
        public HitDirectionEventArgs(HorizontalDirection direction)
        {
            this.direction = direction;
        }

        public HorizontalDirection direction { get; }
    }

    public interface IHitToInteract
    {
        event EventAction<HitDirectionEventArgs> OnHit;
        void Interact(HorizontalDirection direction);

        Vector2 position { get; }

        bool CanBeInteractedWith(Collider2D collider2D);
    }
}