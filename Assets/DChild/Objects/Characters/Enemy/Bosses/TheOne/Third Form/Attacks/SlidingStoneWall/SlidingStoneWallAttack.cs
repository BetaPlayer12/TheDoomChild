using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using System;
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

        [SerializeField]
        private Transform m_arenaCenter;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        private void Awake()
        {
            m_monolithWallLeft.AttackDone += OnAttackDone;
            m_monolithWallRight.AttackDone += OnAttackDone;
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

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
            if (Target.position.x < m_arenaCenter.position.x)
                yield return m_monolithWallRight.CompleteSlidingWallAttackSequence();
            else
                yield return m_monolithWallLeft.CompleteSlidingWallAttackSequence();

            yield return null;
        }
    }

}
