﻿using System.Collections;
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
        private GameObject m_groundChaseTentaclesOne;
        [SerializeField]
        private GameObject m_groundChaseTentaclesTwo;
        [SerializeField]
        private float m_tentacleEmergeInterval;
        [SerializeField]
        private float m_timeBeforeTentacleRetract;

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

            switch (rollAttack)
            {
                case 1:
                    m_currentAttackState.SetState(AttackStyle.Chase);
                    for (int i = 0; i < m_groundChaseTentaclesOne.transform.childCount; i++)
                    {
                        GameObject spawnPoint = m_groundChaseTentaclesOne.transform.GetChild(i).gameObject;
                        spawnPoint.transform.GetChild(0).GetComponent<ChasingGroundTentacle>().ErectTentacle();
                        yield return new WaitForSeconds(m_tentacleEmergeInterval);
                    }

                    yield return new WaitForSeconds(m_timeBeforeTentacleRetract);

                    for (int c = 0; c < m_groundChaseTentaclesOne.transform.childCount; c++)
                    {
                        GameObject spawnPoint = m_groundChaseTentaclesOne.transform.GetChild(c).gameObject;
                        spawnPoint.transform.GetChild(0).GetComponent<ChasingGroundTentacle>().RetractTentacle();
                    }                    
                    break;
                case 2:
                    m_currentAttackState.SetState(AttackStyle.GardenVariationOne);
                    for (int i = 0; i < m_groundChaseTentaclesOne.transform.childCount; i++)
                    {
                        GameObject spawnPoint = m_groundChaseTentaclesOne.transform.GetChild(i).gameObject;
                        spawnPoint.transform.GetChild(0).GetComponent<ChasingGroundTentacle>().ErectTentacle();
                    }

                    yield return new WaitForSeconds(m_timeBeforeTentacleRetract);

                    for (int c = 0; c < m_groundChaseTentaclesOne.transform.childCount; c++)
                    {
                        GameObject spawnPoint = m_groundChaseTentaclesOne.transform.GetChild(c).gameObject;
                        spawnPoint.transform.GetChild(0).GetComponent<ChasingGroundTentacle>().RetractTentacle();
                    }
                    break;
                case 3:
                    m_currentAttackState.SetState(AttackStyle.GardenVariationTwo);
                    for (int i = 0; i < m_groundChaseTentaclesTwo.transform.childCount; i++)
                    {
                        GameObject spawnPoint = m_groundChaseTentaclesOne.transform.GetChild(i).gameObject;
                        spawnPoint.transform.GetChild(0).GetComponent<ChasingGroundTentacle>().ErectTentacle();
                    }

                    yield return new WaitForSeconds(m_timeBeforeTentacleRetract);

                    for (int c = 0; c < m_groundChaseTentaclesOne.transform.childCount; c++)
                    {
                        GameObject spawnPoint = m_groundChaseTentaclesOne.transform.GetChild(c).gameObject;
                        spawnPoint.transform.GetChild(0).GetComponent<ChasingGroundTentacle>().RetractTentacle();
                    }
                    break;
                default:
                    break;
            }           
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        [Button]
        private void GardenAttack()
        {
            for (int i = 0; i < m_groundChaseTentaclesOne.transform.childCount; i++)
            {
                //m_groundChaseTentaclesOne.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<ChasingGroundTentacle>().ErectTentacle();
                GameObject spawnPoint = m_groundChaseTentaclesOne.transform.GetChild(i).gameObject;
                spawnPoint.transform.GetChild(0).GetComponent<ChasingGroundTentacle>().ErectTentacle();
            }

            for (int c = 0; c < m_groundChaseTentaclesOne.transform.childCount; c++)
            {
                m_groundChaseTentaclesOne.transform.GetChild(c).gameObject.transform.GetChild(c).GetComponent<ChasingGroundTentacle>().ErectTentacle();
            }
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            throw new System.NotImplementedException();
        }
    }
}

