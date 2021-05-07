using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class PlayerCollisionSensor : MonoBehaviour
    {
        public event EventAction<EventActionArgs> CollisionDetected;

        private void OnCollisionStay2D(Collision2D collision)
        {
            Debug.Log($"Sensors {collision.collider.name}");
            var playerObject = collision.collider.GetComponentInParent<PlayerControlledObject>();
            if (playerObject != null)
            {
                CollisionDetected?.Invoke(this, EventActionArgs.Empty);
            }
        }
    }
}
