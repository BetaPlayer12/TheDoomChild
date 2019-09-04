using Holysoft;
using Holysoft.Event;
using Holysoft.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Pooling
{
    public abstract class PoolableObject : Actor, IPoolableItem, ISpawnable
    {
        [SerializeField]
        private PoolableItemData m_poolableItemData;

        public PoolableItemData poolableItemData => m_poolableItemData;

        public event EventAction<PoolItemEventArgs> PoolRequest;
        public event EventAction<PoolItemEventArgs> InstanceDestroyed;

        public void DestroyInstance()
        {
            InstanceDestroyed?.Invoke(this, new PoolItemEventArgs(this, transform));
            Destroy(gameObject);
        }

        public virtual void SpawnAt(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public void CallPoolRequest() => PoolRequest?.Invoke(this, new PoolItemEventArgs(this, transform));
    }
}