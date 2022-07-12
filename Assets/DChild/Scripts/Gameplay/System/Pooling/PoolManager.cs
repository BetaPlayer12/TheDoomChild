using DChild.Gameplay.Systems;
using Holysoft.Pooling;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Pooling
{
    public interface IPoolManager
    {
        T GetPool<T>() where T : IPool, new();
    }

    public class PoolManager : SerializedMonoBehaviour, IPoolManager , IGameplaySystemModule
    {
        [OdinSerialize]
        private IPool[] m_poolList;

        public void ClearAll()
        {
            for (int i = 0; i < m_poolList.Length; i++)
            {
                m_poolList[i].Clear();
            }
        }

        public T GetPool<T>() where T : IPool, new()
        {
            var request = typeof(T);
            for (int i = 0; i < m_poolList.Length; i++)
            {
                if (m_poolList[i].GetType() == request)
                {
                    return (T)m_poolList[i];
                }
            }
            throw new Exception($"{typeof(T).Name} does not exist");
        }

        public void Initialize()
        {
            var pooledItemGO = new GameObject("PooledItems");
            SceneManager.MoveGameObjectToScene(pooledItemGO, gameObject.scene);
            ObjectPool.poolItemStorage = pooledItemGO.transform;
            pooledItemGO.SetActive(false);

            for (int i = 0; i < m_poolList.Length; i++)
            {
                m_poolList[i].Initialize();
            }
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            for (int i = 0; i < m_poolList.Length; i++)
            {
                m_poolList[i].Update(deltaTime);
            }
        }
    }
}