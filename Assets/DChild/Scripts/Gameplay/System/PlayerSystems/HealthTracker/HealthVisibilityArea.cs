using UnityEngine;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Systems
{
    public class HealthVisibilityArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var damageable = collision.GetComponentInParent<Damageable>();
            if (damageable)
            {
                Debug.Log(GameplaySystem.healthTracker);
                GameplaySystem.healthTracker.TrackHealth(damageable);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var damageable = collision.GetComponentInParent<Damageable>();
            if (damageable)
            {
                GameplaySystem.healthTracker.RemoveTracker(damageable);
            }
        }
    }
}