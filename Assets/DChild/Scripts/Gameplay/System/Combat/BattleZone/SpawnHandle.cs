using Holysoft.Event;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.BattleZoneComponents
{
    public class SpawnHandle
    {
        private struct Data
        {
            private int m_entityIndex;
            private int m_fxIndex;
            private SpawnInfo.SpawnData m_spawnData;

            public Data(int entityIndex, int fxIndex, SpawnInfo.SpawnData spawnData)
            {
                m_entityIndex = entityIndex;
                m_fxIndex = fxIndex;
                m_spawnData = spawnData;
            }

            public int entityIndex => m_entityIndex;
            public int fxIndex => m_fxIndex;
            public SpawnInfo.SpawnData spawnData => m_spawnData;
        }

        private List<GameObject> m_entitiesToSpawn;
        private List<GameObject> m_fXToSpawn;
        private List<Data> m_toSpawn;
        private float m_timer;

        public EventAction<EventActionArgs> EntitiesFinishSpawning;
        public EventAction<EventActionArgs<GameObject>> EntitySpawned;

        public SpawnHandle()
        {
            m_timer = 0;
            m_entitiesToSpawn = new List<GameObject>();
            m_toSpawn = new List<Data>();
        }

        public void Initialize(SpawnInfo[] infoList)
        {
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
                for (int j = 0; j < info.datas.Length; j++)
                {
                    int fxIndex = -1;
                    fxCache = info.spawnFX;
                    if (fxCache != null)
                    {
                        fxIndex = RecordObject(m_fXToSpawn, fxCache);
                    }
                    m_toSpawn.Add(new Data(entityIndex, fxIndex, info.datas[j]));
                }
            }

            int RecordObject(List<GameObject> recordList, GameObject toRecord)
            {
                if (recordList.Contains(toRecord))
                {
                    return m_fXToSpawn.FindIndex(x => x == fxCache);
                }
                else
                {
                    recordList.Add(toRecord);
                    return m_fXToSpawn.Count - 1;
                }
            }
        }

        public void Update(float delta)
        {
            if (m_toSpawn.Count > 0)
            {
                m_timer += delta;
                for (int i = m_toSpawn.Count - 1; i >= 0; i--)
                {
                    if (m_toSpawn[i].spawnData.spawnDelay < m_timer)
                    {
                        var position = m_toSpawn[i].spawnData.spawnLocation;
                        var instance = Object.Instantiate(m_entitiesToSpawn[m_toSpawn[i].entityIndex], position, Quaternion.identity);
                        if (m_toSpawn[i].fxIndex != -1)
                        {
                            var fx = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_fXToSpawn[m_toSpawn[i].fxIndex], instance.scene);
                            fx.transform.position = position;
                            fx.transform.rotation = Quaternion.identity;
                            fx.Play();
                        }
                        using (Cache<EventActionArgs<GameObject>> cache = Cache<EventActionArgs<GameObject>>.Claim())
                        {
                            cache.Value.Set(instance);
                            EntitySpawned?.Invoke(this, cache.Value);
                            cache.Release();
                        }
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
}