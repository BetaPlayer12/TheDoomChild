using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
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

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        public IEnumerator ExecuteAttack()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            AttackStart?.Invoke(this, EventActionArgs.Empty);

            if (Target.position.x < m_arenaCenter.position.x)
                m_monolithWallRight.SlidingStoneWallAttack();
            else
                m_monolithWallLeft.SlidingStoneWallAttack();

            AttackDone?.Invoke(this, EventActionArgs.Empty);
            yield return null;
        }
    }

}
