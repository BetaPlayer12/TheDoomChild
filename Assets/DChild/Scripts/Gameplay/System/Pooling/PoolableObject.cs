using Holysoft;
using UnityEngine;

namespace DChild.Gameplay.Pooling
{
    public abstract class PoolableObject : Actor, IPoolItem, ISpawnable
    {
        public virtual void DestroyItem() => Destroy(gameObject);
        public virtual void SetParent(Transform parent) => transform.parent = parent;
        public virtual void SpawnAt(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }

}