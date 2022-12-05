using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using Spine.Unity;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleCeilingAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private TentacleCeiling m_leftTentacle;
        [SerializeField]
        private TentacleCeiling m_rightTentacle;

        [SerializeField]
        private float m_ceilingDuration;

        public IEnumerator ExecuteAttack()
        {
            var rollOdds = Random.Range(1, 3);

            //Decide whether to use one or two tentacles to create ceiling
            if(rollOdds == 1)
            {
                var rollSide = Random.Range(1, 3);

                if(rollSide == 1)
                {
                    StartCoroutine(m_leftTentacle.Attack(m_ceilingDuration));
                }
                else if(rollSide == 2)
                {
                    StartCoroutine(m_rightTentacle.Attack(m_ceilingDuration));
                }
            }
            else if(rollOdds == 2)
            {
                StartCoroutine(m_leftTentacle.Attack(m_ceilingDuration));
                StartCoroutine(m_rightTentacle.Attack(m_ceilingDuration));
            }

            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}

