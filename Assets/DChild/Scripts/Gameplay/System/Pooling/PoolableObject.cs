using Holysoft;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Pooling
{
    public abstract class PoolableObject : Actor, IPoolableItem, ISpawnable
    {
        public event EventAction<PoolItemEventArgs> PoolRequest;

        public virtual void DestroyItem() => Destroy(gameObject);
        public virtual void SetParent(Transform parent) => transform.parent = parent;
        public virtual void SpawnAt(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }

}