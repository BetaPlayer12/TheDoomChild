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
            private SpawnInfo.SpawnData m_spawnData;

            public Data(int entityIndex, SpawnInfo.SpawnData spawnData)
            {
                m_entityIndex = entityIndex;
                m_spawnData = spawnData;
            }

            public int entityIndex => m_entityIndex;
            public SpawnInfo.SpawnData spawnData => m_spawnData;
        }

        private List<GameObject> m_entitiesToSpawn;
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
            m_toSpawn.Clear();
            for (int i = 0; i < infoList.Length; i++)
            {
                var info = infoList[i];
                m_entitiesToSpawn.Add(info.entity);
                for (int j = 0; j < info.datas.Length; j++)
                {
                    m_toSpawn.Add(new Data(m_entitiesToSpawn.Count - 1, info.datas[j]));
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