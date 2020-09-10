using DChild.Gameplay.Combat;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay
{

    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField, MinValue(1)]
        private int m_maxSpawns;
        [SerializeField]
        private RangeFloat m_spawnInterval;
        [SerializeField]
        private SpawnArea m_spawnArea;
        [SerializeField]
        private GameObject[] m_possibleSpawns;
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
            var toSpawn = m_possibleSpawns[UnityEngine.Random.Range(0, m_possibleSpawns.Length)];
            var instance = Instantiate(toSpawn) as GameObject;
            instance.transform.position = m_spawnArea.GetRandomPosition();
            m_spawnList.Add(instance);
            instance.GetComponent<Damageable>().Destroyed += OnInstanceDestroyed;
            m_spawnedCount++;
            if (m_spawnedCount >= m_maxSpawns)
            {
                enabled = false;
            }
        }

        private void OnInstanceDestroyed(object sender, EventActionArgs eventArgs)
        {
            var damageable = (Damageable)sender;
            for (int i = 0; i < m_spawnList.Count; i++)
            {
                if (m_spawnList[i] == damageable.gameObject)
                {
                    m_spawnList.RemoveAt(i);
                    m_spawnedCount--;
                    if (enabled == false)
                    {
                        m_spawnTimer = m_spawnInterval.GenerateRandomValue();
                    }
                    enabled = true;
                }
            }
        }

        private void Start()
        {
            m_spawnList = new List<GameObject>();
            m_spawnedCount = 0;
        }

        private void LateUpdate()
        {
            if (m_spawnTimer <= 0)
            {
                SpawnCharacter();
                m_spawnTimer = m_spawnInterval.GenerateRandomValue();
            }
        }
    }
}