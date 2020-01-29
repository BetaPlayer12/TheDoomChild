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
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public virtual void SpawnAt(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

<<<<<<< HEAD
        protected void CallPoolRequest() => PoolRequest?.Invoke(this, new PoolItemEventArgs(this, transform));
=======
        public void CallPoolRequest()
        {
            PoolRequest?.Invoke(this, new PoolItemEventArgs(this, transform));
        }
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
    }
}