using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using Holysoft;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MovingTentacleGroundAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private MovingTentacleGround m_leftTentacle;
        [SerializeField]
        private MovingTentacleGround m_rightTentacle;
        [SerializeField]
        private float m_tentacleMoveSpeed;
        [SerializeField]
        private float m_tentacleAttackDuration;

        public IEnumerator ExecuteAttack()
        {
            var rollTentacle = Random.Range(0, 2);

            if(rollTentacle == 0)
            {
                m_leftTentacle.StartAttack();
            }
            else if(rollTentacle == 1)
            {
                m_rightTentacle.StartAttack();
            }

            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            m_leftTentacle.attackDuration = m_tentacleAttackDuration;
            m_leftTentacle.moveSpeed = m_tentacleMoveSpeed;
            m_rightTentacle.attackDuration = m_tentacleAttackDuration;
            m_rightTentacle.moveSpeed = m_tentacleMoveSpeed;
        }
    }
}

