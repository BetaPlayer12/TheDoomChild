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
        private Transform m_leftTentaclePosition;
        [SerializeField]
        private Transform m_rightTentaclePosition;
        [SerializeField]
        private Transform m_singleTentaclePosition;

        [SerializeField]
        private float m_ceilingTimer;
        private float m_ceilingTimerValue;

        private bool m_createWall;

        public IEnumerator ExecuteAttack()
        {
            m_createWall = true;
            var rollOdds = Random.Range(1, 3);

            //Decide whether to use one or two tentacles to create ceiling
            if(rollOdds == 1)
            {
                var rollSide = Random.Range(1, 3);

                if(rollSide == 1)
                {
                    Debug.Log("Left Tentacle");
                    StartCoroutine(m_leftTentacle.Attack());
                }
                else if(rollSide == 2)
                {
                    Debug.Log("Right Tentacle");
                    StartCoroutine(m_rightTentacle.Attack());

                }
            }
            else if(rollOdds == 2)
            {
                Debug.Log("Left and Right Tentacle");
                StartCoroutine(m_leftTentacle.Attack());
                StartCoroutine(m_rightTentacle.Attack());
            }

            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            if (m_createWall)
            {
                m_ceilingTimer -= GameplaySystem.time.deltaTime;

                if (m_ceilingTimer <= 0)
                {
                    StartCoroutine(m_leftTentacle.Retract());
                    StartCoroutine(m_rightTentacle.Retract());
                    m_ceilingTimer = m_ceilingTimerValue;
                    m_createWall = false;
                }
            }
            
        }

        private void Start()
        {
            m_ceilingTimerValue = m_ceilingTimer;
        }
    }
}

