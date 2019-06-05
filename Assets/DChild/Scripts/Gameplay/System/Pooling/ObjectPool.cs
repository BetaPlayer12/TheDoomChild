/**************************************************
 * 
 * Base class for all Pools
 * 
 ***************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Pooling
{
    [System.Serializable]
    public struct PoolItemEventArgs : IEventActionArgs
    {
        public IPoolableItem item { get; }
    }

    public interface IPoolableItem
    {
        event EventAction<PoolItemEventArgs> PoolRequest;
        void DestroyItem();
        void SetParent(Transform parent);
    }

    public abstract class ObjectPool : IPool
    {
        public static Transform poolItemStorage;

        public abstract void Clear();
        public abstract void Update(float deltaTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of Item To Pool</typeparam>
    /// <typeparam name="U">How to Find the Item</typeparam>
    [System.Serializable]
    public abstract class ObjectPool<T, U> : ObjectPool where T : class, IPoolableItem
    {
        [SerializeField]
        [MinValue(0.1f)]
        private float m_poolDuration;
        protected List<T> m_items;
        protected List<float> m_timers;
        protected int m_poolCount;

        protected abstract int FindIndex(U request);

        public ObjectPool()
        {
            m_poolDuration = 5f;
            m_items = new List<T>();
            m_timers = new List<float>();
            m_poolCount = 0;
        }

        public ObjectPool(float poolDuration)
        {
            m_poolDuration = poolDuration;
            m_items = new List<T>();
            m_timers = new List<float>();
            m_poolCount = 0;
        }

        public override void Update(float deltaTime)
        {
            for (int i = m_poolCount - 1; i >= 0; i--)
            {
                if (m_items[i] == null)
                {
                    m_items.RemoveAt(i);
                    m_timers.RemoveAt(i);
                }
                else
                {
                    m_timers[i] -= deltaTime;
                    if (m_timers[i] <= 0)
                    {
                        var item = RemoveFromPool(i);
                        item.DestroyItem();
                    }
                }
            }
        }

        public override void Clear()
        {
            for (int i = m_poolCount - 1; i >= 0; i--)
            {
                m_items[i].DestroyItem();
                m_items.RemoveAt(i);
                m_timers.RemoveAt(i);
            }
            m_poolCount = 0;
        }

        public void AddToPool(T item)
        {
            item.SetParent(poolItemStorage);
            m_items.Add(item);
            m_timers.Add(m_poolDuration);
            m_poolCount++;
        }

        public T RetrieveFromPool(U request)
        {
            if (m_poolCount > 0)
            {
                var index = FindIndex(request);
                if (index >= 0)
                {
                    var item = RemoveFromPool(index);
                    return item;
                }
            }

            return null;
        }


        protected T RemoveFromPool(int index)
        {
            var item = m_items[index];
            item.SetParent(null);
            m_items.RemoveAt(index);
            m_timers.RemoveAt(index);
            m_poolCount--;
            return item;
        }
    }
}