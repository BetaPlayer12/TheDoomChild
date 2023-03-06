using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MovingTentacleGround : MonoBehaviour
    {
        private Vector2 m_originalPosition;

        [SerializeField]
        private bool m_isLeftTentacle;

        private bool m_startAttack;

        [SerializeField]
        private GameObject m_safeZone;
        [SerializeField]
        private GameObject m_spike;

        [SerializeField]
        private Transform[] m_tentacleObstaclesPositions;

        [SerializeField]
        private Renderer m_renderer;
        private bool m_moveTentacle;

        public float moveSpeed;
        public float attackDuration;
        private float m_attackDurationValue;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        private void Start()
        {
            m_attackDurationValue = attackDuration;
            m_originalPosition = transform.position;
            GenerateSpikesAndSafeZones();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_startAttack)
            {
                attackDuration -= GameplaySystem.time.deltaTime;

                Debug.Log("Attack Duration: " + attackDuration);

                if(attackDuration > 0)
                {
                    if (m_isLeftTentacle)
                        transform.Translate(Vector2.right * moveSpeed * GameplaySystem.time.deltaTime);
                    else
                        transform.Translate(Vector2.left * moveSpeed * GameplaySystem.time.deltaTime);
                }

                if (attackDuration < 0)
                {
                    if (m_renderer.isVisible)
                        transform.Translate(Vector2.down * moveSpeed * GameplaySystem.time.deltaTime);

                    if (!m_renderer.isVisible)
                        ResetTentacle();
                }

                AttackDone?.Invoke(this, EventActionArgs.Empty);
            }           
        }

        public void StartAttack()
        {
            if (FindObjectOfType<ObstacleChecker>().monolithSlamObstacleList != null)
                FindObjectOfType<ObstacleChecker>().ClearMonoliths();
            m_startAttack = true;
        }

        public IEnumerator MoveTentacle()
        {
            m_startAttack = true;

            yield return null;
        }

        private void ResetTentacle()
        {
            attackDuration = m_attackDurationValue;
            transform.position = m_originalPosition;
            DestroyObstacleChildren();
            GenerateSpikesAndSafeZones();
            m_startAttack = false;
        }

        private void GenerateSpikesAndSafeZones()
        { 
            for(int i = 0; i < m_tentacleObstaclesPositions.Length; i++)
            {
                var rollSpikeOrSafeZone = Random.Range(0, 2);

                if(rollSpikeOrSafeZone == 0)
                {
                    InstantiateSpikes(m_tentacleObstaclesPositions[i].position, m_spike, m_tentacleObstaclesPositions[i]);
                }
                else if(rollSpikeOrSafeZone == 1){
                    InstantiateSafeZone(m_tentacleObstaclesPositions[i].position, m_safeZone, m_tentacleObstaclesPositions[i]);
                }
            }
        }

        private void InstantiateSpikes(Vector2 spawnPosition, GameObject spike, Transform parent)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(spike, gameObject.scene);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
            instance.gameObject.transform.SetParent(parent);
        }

        private void InstantiateSafeZone(Vector2 spawnPosition, GameObject safeZone, Transform parent)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(safeZone, gameObject.scene);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
            instance.gameObject.transform.SetParent(parent);
        }

        private void DestroyObstacleChildren()
        {
            for(int i = 0; i < m_tentacleObstaclesPositions.Length; i++)
            {
                m_tentacleObstaclesPositions[i].GetChild(0).GetComponent<PoolableObject>().DestroyInstance();
            }
        }
    }
}

