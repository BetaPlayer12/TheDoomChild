using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkeletonArcherBrain : MinionAIBrain<SkeletonArcher>, IAITargetingBrain
    {

        [SerializeField]
        private CountdownTimer m_attackRest;

        private bool m_isResting;
        private bool m_isAttacking;

        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            else
            {
                m_minion.Stay();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_minion.Stay();
            m_target = null;
            m_attackRest.EndTime(false);
            m_isAttacking = false;
            m_isResting = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_isAttacking)
            {
                m_isAttacking = false;
                m_attackRest.Reset();
                m_isResting = true;
            }
            else if (m_isResting)
            {
                m_attackRest.Tick(m_minion.time.deltaTime);
                m_minion.Stay();
            }
            else
            {
                if (m_target == null)
                {
                    m_minion.Stay();
                }
                else
                {
                    var targetPos = m_target.position;
                    if (IsLookingAt(targetPos))
                    {
                        m_minion.Attack();
                        m_isAttacking = true;
                    }
                    else
                    {
                        m_minion.Turn();
                    }
                }
            }
        }
    }
}
