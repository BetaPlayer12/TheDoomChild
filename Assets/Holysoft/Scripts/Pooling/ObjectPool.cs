using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Pooling
{
    public interface IPool
    {
        void Initialize();
        void Update(float deltaTime);
        void Clear();
    }

    public abstract class ObjectPool : IPool
    {
        public static Transform poolItemStorage;

        public abstract void Clear();
        public abstract void Initialize();
        public abstract void Update(float deltaTime);
    }

    public abstract class ObjectPool<T, U> : ObjectPool where T : class, IPoolableItem
    {
        [SerializeField,MinValue(1)]
        private float m_poolDuration;
        protected List<T> m_items;
        protected List<float> m_timers;

        public override void Initialize()
        {
            m_items = new List<T>();
            m_timers = new List<float>();
        }

        public T GetOrCreateItem(GameObject gameObject)
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                throw new System.Exception($"{gameObject.name} does not have component {typeof(T).Name} for Pool Manager {GetType().Name}");
            }
            else if (m_items.Count > 0)
            {
                var retrievedInstance = RetrieveFromPool(component);
                return retrievedInstance != null ? retrievedInstance : CretateInstance(gameObject);
            }
            else
            {
                return CretateInstance(gameObject);
            }
        }

        private T CretateInstance(GameObject gameObject)
        {
            var instance = UnityEngine.Object.Instantiate(gameObject);
            var newComponent = instance.GetComponent<T>();
            newComponent.PoolRequest += OnPoolRequest;
            newComponent.InstanceDestroyed += OnInstanceDestroyed;
            return newComponent;
        }

        private void OnInstanceDestroyed(object sender, PoolItemEventArgs eventArgs)
        {
            eventArgs.item.PoolRequest -= OnPoolRequest;
            eventArgs.item.InstanceDestroyed -= OnInstanceDestroyed;
        }

        private void OnPoolRequest(object sender, PoolItemEventArgs eventArgs)
        {
            if (eventArgs.hasTransform)
            {
                eventArgs.StoreToPool(poolItemStorage);
            }
            m_items.Add((T)eventArgs.item);
            m_timers.Add(m_poolDuration);
        }

        public override void Clear()
        {
            for (int i = m_items.Count - 1; i >= 0; i--)
            {
                m_items[i].DestroyInstance();
            }
            m_timers.Clear();
        }

        public override void Update(float deltaTime)
        {
            for (int i = m_items.Count - 1; i >= 0; i--)
            {
                m_timers[i] -= deltaTime;
                if (m_timers[i] <= 0)
                {
                    var item = m_items[i];
                    m_items.RemoveAt(i);
                    m_timers.RemoveAt(i);
                    item.DestroyInstance();
                }
            }
        }

        protected T RetrieveFromPool(T component)
        {
            int index = FindAvailableItemIndex(component);
            if (index == -1)
            {
                return null;
            }
            else
            {
                var item = m_items[index];
                m_items.RemoveAt(index);
                m_timers.RemoveAt(index);
                return item;
            }
        }

        protected abstract int FindAvailableItemIndex(T component);

    }
}