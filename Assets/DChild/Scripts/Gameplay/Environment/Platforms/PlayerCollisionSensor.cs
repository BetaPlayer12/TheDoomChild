using DChild.Gameplay.Characters.Players;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class PlayerCollisionSensor : MonoBehaviour, IPlayerWallStickPlatformReaction
    {
        public event EventAction<EventActionArgs> CollisionDetected;

        public void ReactToPlayerWallStick(Character player)
        {
            CollisionDetected?.Invoke(this, EventActionArgs.Empty);
        }

        public void ReactToPlayerWallUnstick(Character player)
        {
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
            {
                CollisionDetected?.Invoke(this, EventActionArgs.Empty);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (GameplaySystem.playerManager.IsPartOfPlayer(collision.gameObject))
            {
                CollisionDetected?.Invoke(this, EventActionArgs.Empty);
            }
        }
    }
}
