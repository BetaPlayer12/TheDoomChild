using Holysoft.Event;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Combat.BattleZoneComponents
{
    [System.Serializable]
    public class SpawnHandle
    {
        private struct Data
        {
            private int m_entityIndex;
            private int m_fxIndex;
            private float m_fxThenInstantiateDelay;
            private SpawnInfo.SpawnData m_spawnData;

            public Data(int entityIndex, int fxIndex, float fxThenInstantiateDelay, SpawnInfo.SpawnData spawnData)
            {
                m_entityIndex = entityIndex;
                m_fxIndex = fxIndex;
                m_fxThenInstantiateDelay = fxThenInstantiateDelay;
                m_spawnData = spawnData;
            }

            public int entityIndex => m_entityIndex;
            public int fxIndex => m_fxIndex;
            public float fxThenInstantiateDelay => m_fxThenInstantiateDelay;
            public SpawnInfo.SpawnData spawnData => m_spawnData;
        }

        private List<GameObject> m_entitiesToSpawn;
        private List<GameObject> m_fXToSpawn;
        private List<Data> m_toSpawn;
        private float m_timer;
        private float m_delayTimer;

        public EventAction<EventActionArgs> EntitiesFinishSpawning;
        public EventAction<EventActionArgs<GameObject>> EntitySpawned;

        public SpawnHandle()
        {
            m_timer = 0;
            m_entitiesToSpawn = new List<GameObject>();
            m_fXToSpawn = new List<GameObject>();
            m_toSpawn = new List<Data>();
        }

        public void Initialize(SpawnInfo[] infoList, float startDelay)
        {
            m_delayTimer = startDelay;
            m_timer = 0;
            m_entitiesToSpawn.Clear();
            m_fXToSpawn.Clear();
            m_toSpawn.Clear();
            GameObject entityCache = null;
            GameObject fxCache = null;
            for (int i = 0; i < infoList.Length; i++)
            {
                var info = infoList[i];
                int entityIndex = -1;
                entityCache = info.entity;
                entityIndex = RecordObject(m_entitiesToSpawn, entityCache);
                int fxIndex = -1;
                fxCache = info.spawnFX;
                if (fxCache != null)
                {
                    fxIndex = RecordObject(m_fXToSpawn, fxCache);
                }
                for (int j = 0; j < info.datas.Length; j++)
                {
                    m_toSpawn.Add(new Data(entityIndex, fxIndex, info.fxThenInstantiateDelay, info.datas[j]));
                }
            }

            int RecordObject(List<GameObject> recordList, GameObject toRecord)
            {
                if (recordList.Contains(toRecord))
                {
                    return recordList.FindIndex(x => x == fxCache);
                }
                else
                {
                    recordList.Add(toRecord);
                    return recordList.Count - 1;
                }
            }
        }

        public void Update(MonoBehaviour coroutineHandler, float delta)
        {
            if (m_delayTimer > 0)
            {
                m_delayTimer -= delta;
            }
            else
            {
                if (m_toSpawn.Count > 0)
                {
                    m_timer += delta;
                    for (int i = m_toSpawn.Count - 1; i >= 0; i--)
                    {
                        if (m_toSpawn[i].spawnData.spawnDelay < m_timer)
                        {
                            var position = m_toSpawn[i].spawnData.spawnLocation;
                            if (m_toSpawn[i].fxIndex != -1)
                            {
                                var fx = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_fXToSpawn[m_toSpawn[i].fxIndex], coroutineHandler.gameObject.scene);
                                fx.transform.position = position;
                                //fx.transform.rotation = Quaternion.identity;
                                fx.Play();
                            }
                            coroutineHandler.StartCoroutine(DelayedSpawn(m_entitiesToSpawn[m_toSpawn[i].entityIndex], position, m_toSpawn[i].fxThenInstantiateDelay));
                            m_toSpawn.RemoveAt(i);
                        }
                    }

                    if (m_toSpawn.Count == 0)
                    {
                        EntitiesFinishSpawning?.Invoke(this, EventActionArgs.Empty);
                    }
                }
            }
        }

        private IEnumerator DelayedSpawn(GameObject gameObject, Vector3 position, float delay)
        {
            yield return new WaitForSeconds(delay);
            var instance = Object.Instantiate(gameObject, position, Quaternion.identity);
            using (Cache<EventActionArgs<GameObject>> cache = Cache<EventActionArgs<GameObject>>.Claim())
            {
                cache.Value.Set(instance);
                EntitySpawned?.Invoke(this, cache.Value);
                cache.Release();
            }
        }
    }
}