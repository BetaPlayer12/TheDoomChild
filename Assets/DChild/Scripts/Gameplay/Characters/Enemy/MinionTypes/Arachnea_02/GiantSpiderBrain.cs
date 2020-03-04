using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantSpiderBrain : MinionAIBrain<GiantSpider>, IAITargetingBrain
    {
        [SerializeField]
        private float m_chaseRange;

        [SerializeField]
        private float m_attackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        private WayPointPatroler m_patrol;
        private bool m_isAttacking;
        private bool m_isResting;

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
            m_patrol.Initialize();
            m_isAttacking = false;
            m_isResting = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(transform.position).destination;

            if (IsLookingAt(destination))
            {
                m_minion.Patrol();
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_patrol = GetComponent<WayPointPatroler>();
            m_patrol.Initialize();
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
                    Patrol();
                }
                else
                {
                    var targetPos = m_target.position;

                    var distance = Vector2.Distance(m_minion.position, targetPos);

                    if(distance <= m_chaseRange)
                    {
                        if (IsLookingAt(targetPos))
                        {
                            if(distance <= m_attackRange)
                            {
                                m_minion.Attack();
                                m_isAttacking = true;
                            }
                            else
                            {
                                m_minion.Move();
                            }
                        }
                        else
                        {
                            m_minion.Turn();
                        }
                    }
                    else
                    {
                        m_target = null;
                    }
                }
            }
        }
    }
}
