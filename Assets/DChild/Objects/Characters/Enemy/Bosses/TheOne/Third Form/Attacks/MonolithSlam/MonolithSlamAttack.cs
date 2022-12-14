using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using System.Linq;
using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MonolithSlamAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private MonolithSlam m_monolith;
        [SerializeField]
        private Transform m_monolithSlamHeight;
        [SerializeField]
        private int m_numOfMonoliths;
        [SerializeField]
        private float m_timeBeforeSmash;
        [SerializeField]
        private float m_spawnIntervalForMonoliths;

        private List<PoolableObject> m_monolithsSpawned = new List<PoolableObject>();

        private bool m_leftToRightSequence;

        public IEnumerator ExecuteAttack()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            int counter = 0;

            m_monolithsSpawned.Clear();

            while (counter < m_numOfMonoliths)
            {
                yield return SetUpMonoliths(Target);
                counter++;
            }

            if (m_leftToRightSequence)
                OrganizeMonolithsSpawnedInDescendingOrder();
            else
                OrganizeMonolithsSpawnedInAscendingOrder();

            if (m_monolithsSpawned.Count > 1)
            {
                int rollMonolithToKeep = Random.Range(0, m_monolithsSpawned.Count);

                m_monolithsSpawned[rollMonolithToKeep].gameObject.GetComponent<MonolithSlam>().keepMonolith = true;
                FindObjectOfType<ObstacleChecker>().monolithSlamObstacleList.Add(m_monolithsSpawned[rollMonolithToKeep]);
            }

            yield return new WaitForSeconds(2f);

            foreach (PoolableObject monolith in m_monolithsSpawned)
            {
                monolith.GetComponent<MonolithSlam>().smashMonolith = true;
                yield return new WaitForSeconds(m_timeBeforeSmash);
            }
        }

        public IEnumerator SetUpMonoliths(AITargetInfo Target)
        {
            InstantiateMonolith(new Vector2(Target.position.x, Target.position.y), m_monolith.gameObject);

            yield return new WaitForSeconds(m_spawnIntervalForMonoliths);
        }

        private void InstantiateMonolith(Vector2 spawnPosition, GameObject monolith)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(monolith, gameObject.scene);
            instance.SpawnAt(new Vector2(spawnPosition.x, m_monolithSlamHeight.position.y), Quaternion.identity);
            m_monolithsSpawned.Add(instance); 
        }

        public void OrganizeMonolithsSpawnedInDescendingOrder()
        {
            m_monolithsSpawned = m_monolithsSpawned.OrderByDescending(x => x.transform.position.x).ToList();
        }

        public void OrganizeMonolithsSpawnedInAscendingOrder()
        {
            m_monolithsSpawned = m_monolithsSpawned.OrderByDescending(x => x.transform.position.x).ToList();
            m_monolithsSpawned.Reverse();
        }
    }
}

