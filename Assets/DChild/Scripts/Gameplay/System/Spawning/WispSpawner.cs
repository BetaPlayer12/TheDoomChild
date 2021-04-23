using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Collections;
using Holysoft.Event;
using Holysoft.Pooling;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{
    public class WispSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_wisp;
        [SerializeField, MinValue(1)]
        private int m_maxSpawns;
        [SerializeField]
        private RangeFloat m_spawnInterval;
        [SerializeField]
        private HorizontalDirection m_spawnDirection;

        private int m_spawnedCount;
        private List<GameObject> m_spawnList;

        private float m_spawnTimer;
        public void ResetSpawner()
        {
            for (int i = m_spawnList.Count - 1; i >= 0; i--)
            {
                Destroy(m_spawnList[i]);
            }
            m_spawnList.Clear();
        }

        private void SpawnCharacter()
        {
            var poolableObject = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_wisp, gameObject.scene);
            var instance = poolableObject.gameObject;
            var character = instance.GetComponent<Character>();
            instance.transform.position = transform.position;
            m_spawnList.Add(instance);
            character.SetFacing(m_spawnDirection);
            var scale = character.transform.localScale;
            scale.x *= (int)m_spawnDirection;
            character.transform.localScale = scale;
            instance.GetComponent<Damageable>().Destroyed += OnInstanceDestroyed;
            instance.GetComponent<PoolableObject>().PoolRequest += OnInstancePooled;
            m_spawnedCount++;
            if (m_spawnedCount >= m_maxSpawns)
            {
                enabled = false;
            }
        }

        private void OnInstancePooled(object sender, PoolItemEventArgs eventArgs)
        {
            var poolableObject = (PoolableObject)sender;
            for (int i = 0; i < m_spawnList.Count; i++)
            {
                if (m_spawnList[i] == poolableObject.gameObject)
                {

                    m_spawnList.RemoveAt(i);
                    m_spawnedCount--;
                    Debug.Log(m_spawnedCount);
                    if (enabled == false)
                    {
                        m_spawnTimer = m_spawnInterval.GenerateRandomValue();
                    }
                    enabled = true;
                }
            }
        }

        private void OnInstanceDestroyed(object sender, EventActionArgs eventArgs)
        {
            var damageable = (Damageable)sender;
            damageable.GetComponent<PoolableObject>().CallPoolRequest();
        }

        private void Start()
        {
            m_spawnList = new List<GameObject>();
            m_spawnedCount = 0;
        }

        private void LateUpdate()
        {
            m_spawnTimer -= GameplaySystem.time.deltaTime;
            if (m_spawnTimer <= 0)
            {
                SpawnCharacter();
                m_spawnTimer = m_spawnInterval.GenerateRandomValue();
            }
        }
    }
}