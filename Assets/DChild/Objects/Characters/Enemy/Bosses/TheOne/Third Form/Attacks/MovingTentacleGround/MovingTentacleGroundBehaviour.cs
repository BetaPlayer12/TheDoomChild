using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MovingTentacleGroundBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float m_moveSpeed;
        [SerializeField]
        private float m_attackDuration;
        private float m_attackDurationValue;

        private Vector2 m_originalPosition;

        [SerializeField]
        private bool m_isFacingRight;

        private bool m_startAttack;

        [SerializeField]
        private GameObject m_safeZone;
        [SerializeField]
        private GameObject m_spike;

        [SerializeField]
        private Transform[] m_tentacleObstaclesPositions;

        private void Start()
        {
            m_attackDurationValue = m_attackDuration;
            m_originalPosition = transform.position;
            GenerateSpikesAndSafeZones();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_startAttack)
            {
                m_attackDuration -= GameplaySystem.time.deltaTime;

                if(m_attackDuration > 0)
                {
                    if (m_isFacingRight)
                    {
                        transform.Translate(Vector2.right * m_moveSpeed * GameplaySystem.time.deltaTime);
                    }
                    else
                    {
                        transform.Translate(Vector2.left * m_moveSpeed * GameplaySystem.time.deltaTime);
                    } 
                }
                else
                {
                    ResetTentacle();
                    m_startAttack = false;
                }
                
            }
            
        }

        private void ResetTentacle()
        {
            m_attackDuration = m_attackDurationValue;
            transform.position = m_originalPosition;
            DestroyChildren();
            GenerateSpikesAndSafeZones();
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

        private void DestroyChildren()
        {
            for(int i = 0; i < m_tentacleObstaclesPositions.Length; i++)
            {
                m_tentacleObstaclesPositions[i].GetChild(0).GetComponent<PoolableObject>().DestroyInstance();
            }
        }

        public void StartAttack()
        {
            m_startAttack = true;
        }
    }
}

