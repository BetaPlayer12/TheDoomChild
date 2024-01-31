using Holysoft.Pooling;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Pooling
{
    public abstract class ObjectPool<T> : ObjectPool where T : MonoBehaviour, IPoolableItem
    {
        [SerializeField, MinValue(1)]
        private float m_poolDuration;
#if UNITY_EDITOR
        [SerializeField, ReadOnly]
#endif
        protected List<T> m_items;
        protected List<float> m_timers;

        public override void Initialize()
        {
            m_items = new List<T>();
            m_timers = new List<float>();
            AddressableSpawner.OnSpawn += OnInstanceCreated;
        }

        public T GetOrCreateItem(GameObject gameObject)
        {
            return GetOrCreateItem(gameObject, Vector3.zero, Quaternion.identity);
        }

        public T GetOrCreateItem(GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                throw new System.Exception($"{gameObject.name} does not have component {typeof(T).Name} for Pool Manager {GetType().Name}");
            }
            else if (m_items.Count > 0)
            {
                var retrievedInstance = RetrieveFromPool(component);
                return retrievedInstance != null ? retrievedInstance : CreateInstance(gameObject, position, rotation);
            }
            else
            {
                return CreateInstance(gameObject, position, rotation);
            }
        }

        public T GetOrCreateItem(GameObject gameObject, Scene scene)
        {
            return GetOrCreateItem(gameObject, scene, Vector3.zero, Quaternion.identity);
        }

        public T GetOrCreateItem(GameObject gameObject, Scene scene, Vector3 position, Quaternion rotation)
        {
            var instance = GetOrCreateItem(gameObject, position, rotation);
            instance.transform.parent = null;
            SceneManager.MoveGameObjectToScene(instance.gameObject, scene);
            return instance;
        }
        public void GetOrCreateItem(AssetReferenceT<GameObject> gameObject, int index = 0, Action<GameObject, int> CallBack = null)
        {

            var component = gameObject.Asset ? ((GameObject)gameObject.Asset).GetComponent<T>() : null;
            if (component != null && m_items.Count > 0)
            {
                var retrievedInstance = RetrieveFromPool(component);
                if (retrievedInstance == null)
                {
                    CreateInstance(gameObject, index, CallBack);
                }
                else
                {
                    CallBack(retrievedInstance.gameObject, index);
                }
            }
            else
            {
                CreateInstance(gameObject, index, CallBack);
            }
        }

        private void CreateInstance(AssetReferenceT<GameObject> gameObject, int index = 0, Action<GameObject, int> CallBack = null)
        {
            AddressableSpawner.Spawn(gameObject, Vector3.zero, index, CallBack);
        }

        private T CreateInstance(GameObject gameObject)
        {
            return CreateInstance(gameObject, Vector3.zero, Quaternion.identity);
        }

        private T CreateInstance(GameObject gameObject, Vector3 position, Quaternion rotation)
        {
            var instance = UnityEngine.Object.Instantiate(gameObject, position, rotation);
            var newComponent = instance.GetComponent<T>();
            newComponent.PoolRequest += OnPoolRequest;
            newComponent.InstanceDestroyed += OnInstanceDestroyed;
            return newComponent;
        }

        private void OnInstanceCreated(GameObject obj)
        {
            if (obj.TryGetComponent(out T newComponent))
            {
                newComponent.PoolRequest += OnPoolRequest;
                newComponent.InstanceDestroyed += OnInstanceDestroyed;
            }
        }

        protected void OnInstanceDestroyed(object sender, PoolItemEventArgs eventArgs)
        {
            eventArgs.item.PoolRequest -= OnPoolRequest;
            eventArgs.item.InstanceDestroyed -= OnInstanceDestroyed;
        }

        protected void OnPoolRequest(object sender, PoolItemEventArgs eventArgs)
        {
            if (eventArgs.hasTransform)
            {
                eventArgs.StoreToPool(poolItemStorage);
            }
            var item = (T)eventArgs.item;
            if (m_items.Contains(item) == false)
            {
                m_items.Add(item);
                m_timers.Add(m_poolDuration);
            }
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

        protected virtual T RetrieveFromPool(T component)
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

        protected virtual int FindAvailableItemIndex(T component)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                if (m_items[i] != null && m_items[i].poolableItemData == component.poolableItemData)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}