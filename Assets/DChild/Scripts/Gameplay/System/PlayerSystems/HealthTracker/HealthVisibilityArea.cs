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
                //Add UI to thingy
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var damageable = collision.GetComponentInParent<Damageable>();
            if (damageable)
            {
                //Remove UI to thingy
            }
        }
    }
}