using UnityEngine;

namespace DChild.Gameplay.Pooling
{
    [RequireComponent(typeof(Collider2D))]
    public class ForcePoolArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var spawnable = collision.GetComponentInParent<IForcePool>();
            if (spawnable != null)
            {
                spawnable.PoolObject();
            }
        }

        private void OnValidate()
        {
            gameObject.name = "ForcePoolArea";
            var collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }
    }
}