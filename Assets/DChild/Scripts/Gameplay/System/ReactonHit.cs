using DChild.Gameplay.Characters;
using DChild.Gameplay.Environment.Interractables;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class ReactonHit : MonoBehaviour, IHitToInteract
    {
        public Vector2 position => transform.position;

        public event EventAction<HitDirectionEventArgs> OnHit;

        public bool CanBeInteractedWith(Collider2D collider2D)
        {
            return true;
        }
        public void Interact(HorizontalDirection direction)
        {
            OnHit?.Invoke(this, new HitDirectionEventArgs(direction));
        }
    }
}
