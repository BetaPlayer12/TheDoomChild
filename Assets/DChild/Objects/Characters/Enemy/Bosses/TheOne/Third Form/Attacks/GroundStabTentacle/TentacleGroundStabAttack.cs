using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Spine.Unity;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleGroundStabAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private GameObject[] m_tentaclesAtBackground;
        [SerializeField]
        private GameObject[] m_tentaclesAtMidground;
        [SerializeField]
        private GameObject[] m_tentaclesAtPlayableArea;
        [SerializeField]
        private GameObject[] m_tentaclesAtForeground;

        private int m_tentacleCount = 0;
        private Vector2 m_tentacleOffset = new Vector2(0, 50f);

        [SerializeField]
        private float m_tentacleStabTimer = 0;
        private float m_tentacleStabTimerValue;

        [SerializeField]
        private int m_backgroundSortingLayerID = -2;

        public IEnumerator ExecuteAttack()
        {
            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            m_tentacleCount++;
            switch (m_tentacleCount)
            {
                case 1:
                    InstantiateTentacles(PlayerPosition + m_tentacleOffset, m_tentaclesAtBackground[Random.Range(0, m_tentaclesAtBackground.Length)]);
                    yield return new WaitForSeconds(2f);
                    break;
                case 2:
                    InstantiateTentacles(PlayerPosition + m_tentacleOffset, m_tentaclesAtMidground[Random.Range(0, m_tentaclesAtMidground.Length)]);
                    yield return new WaitForSeconds(2f);
                    break;
                case 3:
                    InstantiateTentacles(PlayerPosition + m_tentacleOffset, m_tentaclesAtPlayableArea[Random.Range(0, m_tentaclesAtPlayableArea.Length)]);
                    yield return new WaitForSeconds(2f);
                    break;
                case 4:
                    InstantiateTentacles(PlayerPosition + m_tentacleOffset, m_tentaclesAtForeground[Random.Range(0, m_tentaclesAtForeground.Length)]);
                    yield return new WaitForSeconds(2f);
                    break;
                case 5:
                    m_tentacleCount = 0;
                    break;
                default:
                    break;
            }

            yield return null;
        }

        private void InstantiateTentacles(Vector2 spawnPosition, GameObject tentacle)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(tentacle, gameObject.scene);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
            //StartCoroutine(instance.GetComponent<TentacleLifeSpan>().StabRoutine());
        }

        private void Start()
        {
            m_tentacleStabTimerValue = m_tentacleStabTimer;

            foreach(GameObject tentacle in m_tentaclesAtBackground)
            {
                tentacle.GetComponentInChildren<MeshRenderer>().sortingLayerID = m_backgroundSortingLayerID;
                tentacle.GetComponentInChildren<MeshRenderer>().sortingLayerName = "Background";
            }
        }

        private void Update()
        {
            m_tentacleStabTimer -= GameplaySystem.time.deltaTime;

            if (m_tentacleStabTimer <= 0)
            {
                //StartCoroutine(m_tentacleStabAttack.ExecuteAttack(m_targetInfo.position));
                m_tentacleStabTimer = m_tentacleStabTimerValue;
            }
        }
    }
}

