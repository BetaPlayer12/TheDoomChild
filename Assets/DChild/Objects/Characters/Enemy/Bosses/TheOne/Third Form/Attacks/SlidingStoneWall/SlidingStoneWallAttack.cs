using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SlidingStoneWallAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField]
        private SlidingStoneWall m_monolithWallLeft;
        [SerializeField]
        private SlidingStoneWall m_monolithWallRight;
        //[SerializeField]
        //private Transform m_leftSpawnPoint;
        //[SerializeField]
        //private Transform m_rightSpawnPoint;
        [SerializeField]
        private Transform m_arenaCenter;

        public IEnumerator ExecuteAttack()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
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

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            if (Target.position.x < m_arenaCenter.position.x)
                m_monolithWallRight.SlidingStoneWallAttack();
            else
                m_monolithWallLeft.SlidingStoneWallAttack();

            yield return null;
        }
    }

}
