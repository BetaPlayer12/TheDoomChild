using System;
using System.Collections.Generic;

namespace DChild.Gameplay.Pooling
{
    public class PoolableObjectPool : ObjectPool<PoolableObject>
    {
        private List<PoolableObject> m_registeredItems;

        public override void Initialize()
        {
            base.Initialize();
            m_registeredItems = new List<PoolableObject>();
        }

        public void Register(PoolableObject poolable)
        {
            if (m_registeredItems.Contains(poolable) == false)
            {
                poolable.PoolRequest += OnPoolRequest;
                poolable.InstanceDestroyed += OnInstanceDestroyed;
                m_registeredItems.Add(poolable);
            }
        }

        public void Unregister(PoolableObject poolable)
        {
            if (m_registeredItems.Contains(poolable))
            {
                poolable.PoolRequest -= OnPoolRequest;
                poolable.InstanceDestroyed -= OnInstanceDestroyed;
                m_registeredItems.Remove(poolable);
            }
        }
    }
}