using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleCeilingAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private GameObject m_leftTentacle;
        [SerializeField]
        private GameObject m_rightTentacle;

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
                    m_leftTentacle.SetActive(true);
                    m_leftTentacle.transform.position = m_singleTentaclePosition.position;
                }
                else if(rollSide == 2)
                {
                    Debug.Log("Right Tentacle");
                    m_rightTentacle.SetActive(true);
                    m_rightTentacle.transform.position = m_singleTentaclePosition.position;
                }
            }
            else if(rollOdds == 2)
            {
                Debug.Log("Left and Right Tentacle");
                m_leftTentacle.SetActive(true);
                m_leftTentacle.transform.position = m_leftTentaclePosition.position;

                m_rightTentacle.SetActive(true);
                m_rightTentacle.transform.position = m_rightTentaclePosition.position;
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
                    m_leftTentacle.SetActive(false);
                    m_rightTentacle.SetActive(false);

                    m_ceilingTimer = m_ceilingTimerValue;
                    m_createWall = false;
                }
            }
            
        }

        private void Start()
        {
            m_leftTentacle.SetActive(false);
            m_rightTentacle.SetActive(false);

            m_ceilingTimerValue = m_ceilingTimer;
        }
    }
}

