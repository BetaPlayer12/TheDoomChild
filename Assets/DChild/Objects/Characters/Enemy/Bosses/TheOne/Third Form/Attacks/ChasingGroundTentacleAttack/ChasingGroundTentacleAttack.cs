using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Characters.AI;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ChasingGroundTentacleAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private GameObject m_groundChaseTentacle;
        [SerializeField]
        private Transform[] m_groundChaseTentacleSpawnPoints;
        [SerializeField]
        private Transform[] m_gardenTentacleVariationOneSpawnPoints;
        [SerializeField]
        private Transform[] m_gardenTentacleVariationTwoSpawnPoints;

        [ShowInInspector]
        private StateHandle<AttackStyle> m_currentAttackState;

        private enum AttackStyle
        {
            Chase,
            GardenVariationOne,
            GardenVariationTwo,
        }

        public IEnumerator ExecuteAttack()
        {
            var rollAttack = Random.Range(1, 4);

            Debug.Log(rollAttack);

            switch (rollAttack)
            {
                case 1:
                    m_currentAttackState.SetState(AttackStyle.Chase);
                    for (int i = 0; i < m_groundChaseTentacleSpawnPoints.Length; i++)
                    {
                        InstantiateTentacles(m_groundChaseTentacleSpawnPoints[i].position, m_groundChaseTentacle);
                        yield return new WaitForSeconds(2f);
                    }
                    break;
                case 2:
                    m_currentAttackState.SetState(AttackStyle.GardenVariationOne);
                    for (int i = 0; i < m_groundChaseTentacleSpawnPoints.Length; i++)
                    {
                        InstantiateTentacles(m_gardenTentacleVariationOneSpawnPoints[i].position, m_groundChaseTentacle);
                    }
                    break;
                case 3:
                    m_currentAttackState.SetState(AttackStyle.GardenVariationTwo);
                    for (int i = 0; i < m_groundChaseTentacleSpawnPoints.Length; i++)
                    {
                        InstantiateTentacles(m_gardenTentacleVariationTwoSpawnPoints[i].position, m_groundChaseTentacle);
                    }
                    break;
                default:
                    break;
            }

            //switch (m_currentAttackState.currentState)
            //{
            //    case AttackStyle.Chase:
            //        for (int i = 0; i < m_groundChaseTentacleSpawnPoints.Length; i++)
            //        {
            //            InstantiateTentacles(m_groundChaseTentacleSpawnPoints[i].position, m_groundChaseTentacle);
            //            yield return new WaitForSeconds(2f);
            //        }
            //        break;
            //    case AttackStyle.GardenVariationOne:
            //        for (int i = 0; i < m_groundChaseTentacleSpawnPoints.Length; i++)
            //        {
            //            InstantiateTentacles(m_gardenTentacleVariationOneSpawnPoints[i].position, m_groundChaseTentacle);
            //        }
            //        break;
            //    case AttackStyle.GardenVariationTwo:
            //        for (int i = 0; i < m_groundChaseTentacleSpawnPoints.Length; i++)
            //        {
            //            InstantiateTentacles(m_gardenTentacleVariationTwoSpawnPoints[i].position, m_groundChaseTentacle);
            //        }
            //        break;
            //}
            
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        private void InstantiateTentacles(Vector2 spawnPosition, GameObject tentacle)
        {
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(tentacle, gameObject.scene);
            instance.SpawnAt(spawnPosition, Quaternion.identity);
        }
    }
}

