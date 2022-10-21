using System.Collections;
using System.Collections.Generic;
using Holysoft;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MovingTentacleGroundAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private MovingTentacleGroundBehaviour m_leftTentacle;
        [SerializeField]
        private MovingTentacleGroundBehaviour m_rightTentacle;

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

        private void Update()
        {
            
        }
    }
}

