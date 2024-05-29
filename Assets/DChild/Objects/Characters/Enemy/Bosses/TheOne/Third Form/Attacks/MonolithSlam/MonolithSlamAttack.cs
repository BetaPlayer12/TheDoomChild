using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using System.Linq;
using DChild.Gameplay.Characters.AI;
using Sirenix.OdinInspector;
using Holysoft.Event;

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
        [SerializeField]
        private float m_spawnOffset;

        [SerializeField]
        private List<PoolableObject> m_monolithsSpawned = new List<PoolableObject>();
        [SerializeField]
        private List<float> m_monolithsSpawnedXPositions = new List<float>();
        private ObstacleChecker m_obstacleChecker;

        [SerializeField, BoxGroup("tentacleValues")]
        private List<Transform> m_tentaclePosition = new List<Transform>(5);

        [SerializeField, BoxGroup("tentacleValues")]
        List<float> m_waitValues = new List<float>();

        List<int> m_positionsUsed = new List<int>();
        List<float> m_DurationUsed = new List<float>();

        private bool m_leftToRightSequence;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

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
            AttackStart?.Invoke(this, EventActionArgs.Empty);
            //Initialize monoliths to spawn and clear spawned monoliths 
            int counter = 0;

            m_monolithsSpawned.Clear();

            while (counter<=4)
            {
                yield return SetUpMonoliths(Target,counter);
                counter++;
            }

            //Organize Monoliths to drop in correct order of left to right or right to left
            if (m_leftToRightSequence)
                OrganizeMonolithsSpawnedInDescendingOrder();
            else
                OrganizeMonolithsSpawnedInAscendingOrder();

            //Anticipation time before smashing monoliths
            yield return new WaitForSeconds(1f);

            /*
            //Pick a monolith to keep as platform
            if (m_monolithsSpawned.Count > 1)
            {
                int rollMonolithToKeep = Random.Range(0, m_monolithsSpawned.Count);

                m_monolithsSpawned[rollMonolithToKeep].gameObject.GetComponent<MonolithSlam>().keepMonolith = true;

                m_obstacleChecker.AddMonolithToList(m_monolithsSpawned[rollMonolithToKeep]);
            }
            */

            

            //Set smashMonolith true in each monolith to trigger smash
            foreach (PoolableObject monolith in m_monolithsSpawned)
            {
                if(monolith != null)
                    monolith.GetComponent<MonolithSlam>().TriggerSmash();
                //yield return new WaitForSeconds(m_timeBeforeSmash);
            }
            m_DurationUsed.Clear();
            //Anticipation time before smashing monoliths
            yield return new WaitForSeconds(6f);

            while (counter < m_numOfMonoliths)
            {
                yield return SetUpMonoliths(Target, counter);
                counter++;
            }

            //Anticipation time before smashing monoliths
            yield return new WaitForSeconds(1f);
            //Set smashMonolith true in each monolith to trigger smash
            foreach (PoolableObject monolith in m_monolithsSpawned)
            {
                if (monolith != null)
                {
                    monolith.GetComponent<MonolithSlam>().keepMonolith=false;
                    monolith.GetComponent<MonolithSlam>().TriggerSmash();
                }
                    
                //yield return new WaitForSeconds(m_timeBeforeSmash);
            }
            yield return new WaitForSeconds(6f);
            m_positionsUsed.Clear();
            m_monolithsSpawned.Clear();
            m_monolithsSpawnedXPositions.Clear();
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_obstacleChecker = FindObjectOfType<ObstacleChecker>();
        }

        public IEnumerator SetUpMonoliths(AITargetInfo Target,int counter)
        {
            Debug.Log(counter);
            if(counter>=5)
            {

                int x = Random.Range(0, 4);
                while (m_positionsUsed.Contains(x))
                {
                    x = Random.Range(0, 4);
                }
                m_positionsUsed.Add(x);
                InstantiateMonolith(m_tentaclePosition[x].position, m_monolith.gameObject);
            }else
            {
                InstantiateMonolith(m_tentaclePosition[counter].position, m_monolith.gameObject);
            }
            /*
            if (m_monolithsSpawnedXPositions.Contains(Target.position.x))
            {
                int randomRoll = Random.Range(0, 2);
                if (randomRoll == 0)
                {
                    InstantiateMonolith(new Vector2(m_monolithsSpawnedXPositions[m_monolithsSpawnedXPositions.Count - 1] + m_spawnOffset, Target.position.y), m_monolith.gameObject);
                }
                else
                {
                    InstantiateMonolith(new Vector2(m_monolithsSpawnedXPositions[m_monolithsSpawnedXPositions.Count - 1] + m_spawnOffset, Target.position.y), m_monolith.gameObject);
                }
            }
            else
            {
                InstantiateMonolith(new Vector2(Target.position.x, Target.position.y), m_monolith.gameObject);
            }
            */
            //InstantiateMonolith(new Vector2(Target.position.x, Target.position.y), m_monolith.gameObject);
            yield return new WaitForSeconds(m_spawnIntervalForMonoliths);
        }
        /*
        public IEnumerator SetUpMonoliths(AITargetInfo Target)
        {          
            if (m_monolithsSpawnedXPositions.Contains(Target.position.x))
            {
                int randomRoll = Random.Range(0, 2);
                if (randomRoll == 0)
                {
                    InstantiateMonolith(new Vector2(m_monolithsSpawnedXPositions[m_monolithsSpawnedXPositions.Count - 1] + m_spawnOffset, Target.position.y), m_monolith.gameObject);
                }
                else
                {
                    InstantiateMonolith(new Vector2(m_monolithsSpawnedXPositions[m_monolithsSpawnedXPositions.Count - 1] + m_spawnOffset, Target.position.y), m_monolith.gameObject);
                }
            }
            else
            {
                InstantiateMonolith(new Vector2(Target.position.x, Target.position.y), m_monolith.gameObject);
            }

            //InstantiateMonolith(new Vector2(Target.position.x, Target.position.y), m_monolith.gameObject);
            yield return new WaitForSeconds(m_spawnIntervalForMonoliths);
        }
        */
        private void InstantiateMonolith(Vector2 spawnPosition, GameObject monolith)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(monolith, gameObject.scene);
            instance.SpawnAt(new Vector2(spawnPosition.x, m_monolithSlamHeight.position.y), Quaternion.identity);
            int index = Random.Range(0, m_waitValues.Count - 1) ;
            int counter = 0;
            while(m_DurationUsed.Contains(index)&&counter<10)
            {
                index = Random.Range(0, m_waitValues.Count - 1);
                counter++;
                if(counter>=10)
                {
                    index = 2;
                }
            }
            m_DurationUsed.Add(index);
            float waitVal = m_waitValues[index];
            instance.GetComponent<MonolithSlam>().SetTentacleHoldDuration(waitVal);
            m_monolithsSpawnedXPositions.Add(instance.transform.position.x);
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

        [Button]
        private void TestMonolithSlam()
        {
            AITargetInfo dummy = new AITargetInfo();
            StartCoroutine(ExecuteAttack(dummy));
        }
    }
}

