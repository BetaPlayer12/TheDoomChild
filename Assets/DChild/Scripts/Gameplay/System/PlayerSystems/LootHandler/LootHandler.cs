using DChild.Gameplay.Pooling;
using DChild.Gameplay.SoulEssence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class LootHandler : MonoBehaviour, ILootHandler, IGameplaySystemModule
    {
        [SerializeField, Min(1)]
        private int m_maxLootSpawnPerFrame;
        private List<LootDropRequest> m_requests;

        public void DropLoot(LootDropRequest request)
        {
            m_requests.Add(request);
            enabled = true;
        }

        public void ClearRequests()
        {
            m_requests.Clear();
            enabled = false;
        }

        private void HandleRequests()
        {
            var availableInstanceSpawnCount = m_maxLootSpawnPerFrame;
            do
            {
                var request = m_requests[0];
                availableInstanceSpawnCount -= HandleRequest(ref request, availableInstanceSpawnCount);
                if (request.count == 0)
                {
                    m_requests.RemoveAt(0);
                }
                else
                {
                    m_requests[0] = request;
                }
            } while (availableInstanceSpawnCount > 0);
        }

        private int HandleRequest(ref LootDropRequest request, int maxInstance)
        {
            if (request.count >= m_maxLootSpawnPerFrame)
            {
                for (int i = 0; i < maxInstance; i++)
                {
                    GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(request.loot).GetComponent<Loot>().SpawnAt(request.location, Quaternion.identity);
                }
                request.count -= maxInstance;
                return maxInstance;
            }
            else
            {

                var instanceCount = request.count;
                for (int i = 0; i < instanceCount; i++)
                {
                    Instantiate(request.loot).GetComponent<Loot>().SpawnAt(request.location, Quaternion.identity);
                }
                request.count = 0;
                return instanceCount;
            }
        }

        private void Awake()
        {
            m_requests = new List<LootDropRequest>();
            enabled = false;
        }

        private void Update()
        {
            HandleRequests();
            if (m_requests.Count <= 0)
            {
                enabled = false;
            }
        }
    }
}