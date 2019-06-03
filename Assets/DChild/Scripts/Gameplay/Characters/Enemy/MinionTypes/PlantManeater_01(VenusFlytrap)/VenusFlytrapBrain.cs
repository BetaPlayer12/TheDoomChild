using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class VenusFlytrapBrain : MinionAIBrain<VenusFlytrap>, IAITargetingBrain
    {
        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private float m_BiteAttackDistance;

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
                m_minion.Idle();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_minion.Idle();
            m_target = null;
            m_attackRest.EndTime(false);
            m_isAttacking = false;
            m_isResting = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            if (target != null)
            {
                m_target = target;
            }
            else
            {
                m_target = null;
            }
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
                m_minion.Idle();
            }
            else
            {
                if (m_target == null)
                {
                    m_minion.Idle();
                }
                else
                {
                    var targetPos = m_target.position;
                    if (IsLookingAt(targetPos))
                    {
                        var distance = Vector2.Distance(m_minion.position, targetPos);

                        if(distance <= m_BiteAttackDistance)
                        {
                            m_minion.BiteAttack(targetPos);
                        }
                        else
                        {
                            m_minion.WhipAttack(targetPos);
                        }

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
