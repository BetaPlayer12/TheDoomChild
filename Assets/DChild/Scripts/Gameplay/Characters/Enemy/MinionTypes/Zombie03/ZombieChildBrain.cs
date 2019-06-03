using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ZombieChildBrain : MinionAIBrain<ZombieChild>, IAITargetingBrain
    {
        [SerializeField]
        private float m_scratchAttackRange;

        [SerializeField]
        private float m_vomitAttackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private CountdownTimer m_triggerVomit;

        private WayPointPatroler m_patrol;

        private bool m_isAttacking;
        private bool m_isResting;
        private bool m_isToPatrol;
        private bool m_isOnStandBy;
        private bool m_isToVomit;

        public bool OnPatrol;


        private void Patrol()
        {
            var destination = m_patrol.GetInfo(transform.position).destination;

            if (IsLookingAt(destination))
            {
                m_minion.Patrol(destination);
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void MoveTo(Vector2 targetpos)
        {
            m_minion.MoveTo(targetpos);
        }

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
            m_attackRest.EndTime(false);
            m_triggerVomit.EndTime(false);
            m_target = null;
            m_isResting = false;
            m_isAttacking = false;
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

            if (m_isOnStandBy && m_target != null)
            {
                m_minion.Detect();
                m_isOnStandBy = false;
            }
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;
        private void OnTriggerVomitEnd(object sender, EventActionArgs eventArgs) => m_isToVomit = true;
        
        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_triggerVomit.CountdownEnd += OnTriggerVomitEnd;
        }

        private void OnValidate()
        {
            if (OnPatrol)
            {
                m_isToPatrol = OnPatrol;
            }
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
            else if (m_target == null)
            {
                m_isOnStandBy = true;

                if (m_isToVomit)
                {
                    m_minion.Vomit();
                    m_isToVomit = false;
                    m_triggerVomit.Reset();
                }
                else
                {
                    m_triggerVomit.Tick(m_minion.time.deltaTime);

                    if (m_isToPatrol)
                    {
                        Patrol();
                    }
                    else
                    {
                        m_minion.Stay();
                    }
                }   
            }
            else
            {
                var targetPosition = m_target.position;

                if (IsLookingAt(targetPosition))
                {
                    var distance = Vector2.Distance(m_minion.position, targetPosition);
                    if (distance <= m_scratchAttackRange)
                    {
                        m_minion.ScratchAttack();
                        m_isAttacking = true;
                    }
                    else if (distance <= m_vomitAttackRange)
                    {
                        m_minion.VomitAttack();
                        m_isAttacking = true;
                    }
                    else
                    {
                        MoveTo(targetPosition);
                    }
                }
                else
                {
                    m_minion.Turn();
                }
            }
        }
    }
}
