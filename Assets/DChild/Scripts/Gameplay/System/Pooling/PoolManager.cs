using DChild.Gameplay.Systems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Pooling
{
    public interface IPoolManager
    {
        T GetOrCreatePool<T>() where T : IPool, new();
    }

    public class PoolManager : MonoBehaviour, IPoolManager , IGameplaySystemModule
    {
        private List<IPool> m_poolList;

        public void ClearAll()
        {
            for (int i = 0; i < m_poolList.Count; i++)
            {
                m_poolList[i].Clear();
            }
        }

        public T GetOrCreatePool<T>() where T : IPool, new()
        {
            var request = typeof(T);
            for (int i = 0; i < m_poolList.Count; i++)
            {
                if (m_poolList[i].GetType() == request)
                {
                    return (T)m_poolList[i];
                }
            }

            //If pool is being requested 
            var pool = new T();
            m_poolList.Add(pool);
            return pool;
        }

        private void Awake()
        {
            m_poolList = new List<IPool>();
            var pooledItemGO = new GameObject("PooledItems");
            SceneManager.MoveGameObjectToScene(pooledItemGO, gameObject.scene);
            ObjectPool.poolItemStorage = pooledItemGO.transform;
            pooledItemGO.SetActive(false);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            for (int i = 0; i < m_poolList.Count; i++)
            {
                m_poolList[i].Update(deltaTime);
            }
        }
    }

}