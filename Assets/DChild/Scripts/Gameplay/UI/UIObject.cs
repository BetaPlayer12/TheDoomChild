using Holysoft;
using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holysoft.Event;
using Holysoft.Pooling;

namespace DChild.UI
{
    public abstract class UIObject : Actor, IPoolableItem
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

        protected void CallPoolRequest() => PoolRequest?.Invoke(this, new PoolItemEventArgs(this, transform));
    }

}