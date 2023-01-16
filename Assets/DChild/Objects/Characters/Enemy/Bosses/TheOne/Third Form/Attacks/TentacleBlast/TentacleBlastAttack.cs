using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleBlastAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private TentacleBlast m_leftTentacleBlast;
        [SerializeField]
        private TentacleBlast m_rightTentacleBlast;

        public IEnumerator ExecuteAttack()
        {
            int rollSide = Random.Range(0, 3);
            switch (rollSide)
            {
                case 0:
                    yield return m_leftTentacleBlast.TentacleBlastAttack();
                    break;
                case 1:
                    yield return m_rightTentacleBlast.TentacleBlastAttack();
                    break;
                case 2:
                    StartCoroutine(m_leftTentacleBlast.TentacleBlastAttack());
                    StartCoroutine(m_rightTentacleBlast.TentacleBlastAttack());
                    break;
                default:
                    yield return m_leftTentacleBlast.TentacleBlastAttack();
                    break;

            }
            
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

