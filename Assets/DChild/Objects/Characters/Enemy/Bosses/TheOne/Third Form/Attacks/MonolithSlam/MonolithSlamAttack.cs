using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using System.Linq;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MonolithSlamAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private MonolithSlam m_monolith;

        private List<PoolableObject> m_monolithsSpawned;

        private bool m_leftToRightSequence;

        public IEnumerator ExecuteAttack()
        {
            if (m_leftToRightSequence)
            {
                OrganizeMonolithsSpawnedInDescendingOrder();
                
            }
            else
            {
                OrganizeMonolithsSpawnedInAscendingOrder();
            }

            foreach (PoolableObject monolith in m_monolithsSpawned)
            {
                monolith.GetComponent<MonolithSlam>().smashMonolith = true;
            }


            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator SetUpMonoliths(Vector2 PlayerPosition)
        {
            InstantiateMonolith(PlayerPosition, m_monolith.gameObject);

            yield return new WaitForSeconds(3f);
        }

        private void InstantiateMonolith(Vector2 spawnPosition, GameObject monolith)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(monolith, gameObject.scene);
            instance.SpawnAt(new Vector2(spawnPosition.x, spawnPosition.y + 10f), Quaternion.identity);
            m_monolithsSpawned.Add(instance);            
        }

        public void OrganizeMonolithsSpawnedInDescendingOrder()
        {
            m_monolithsSpawned = m_monolithsSpawned.OrderByDescending(x => x.transform.position.x).ToList();
        }

        public void OrganizeMonolithsSpawnedInAscendingOrder()
        {
            //flip this j
            m_monolithsSpawned = m_monolithsSpawned.OrderByDescending(x => x.transform.position.x).ToList();
            m_monolithsSpawned.Reverse();
        }

      
    }
}

