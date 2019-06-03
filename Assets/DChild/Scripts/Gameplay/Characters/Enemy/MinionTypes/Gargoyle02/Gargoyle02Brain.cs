using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Spine.Unity.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Gargoyle02Brain : MinionAIBrain<Gargoyle02>, IAITargetingBrain
    {
        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private CountdownTimer m_patrolTime;

        [SerializeField]
        private float m_attackRange;

        private WayPointPatroler m_patrol;
        private Vector2 m_initialPosition;

        private bool m_isInStoneState;
        private bool m_isMovingBackToPosition;
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
            m_target = null;
            m_isAttacking = false;
            m_isInStoneState = true;
            m_isResting = false;
            m_isMovingBackToPosition = false;
            m_initialPosition = transform.position;
            m_attackRest.EndTime(false);
            m_patrolTime.EndTime(false);
        }

        public void SetTarget(IEnemyTarget target)
        {
            if (target != null)
            {
                m_target = target;
                m_patrolTime.Reset();
            }
            else
            {
                m_target = null;
            }

            if (m_isInStoneState)
            {
                m_isInStoneState = false;
                m_minion.PlayerDetected();
            }
        }

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(m_minion.position).destination;
            if (IsLookingAt(destination))
            {
                m_minion.Patrol(destination);
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void MoveTo(Vector2 targetPos)
        {
            m_minion.MoveTo(targetPos);
        }

        private void Attack()
        {
            var rand = Mathf.Abs(Random.Range(1, 3));
            switch (rand)
            {
                case 1:
                    m_minion.ClawAttack();
                    break;
                case 2:
                    m_minion.WingAttack();
                    break;
            }
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;
        private void OnPatrolRestEnd(object sender, EventActionArgs eventArgs) => m_isMovingBackToPosition = true;
        
        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_initialPosition = transform.position;
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_patrolTime.CountdownEnd += OnPatrolRestEnd;
            m_isInStoneState = true;
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
            else if (m_isMovingBackToPosition)
            {
                m_minion.MoveTo(m_initialPosition);

                if(m_minion.position.x <= m_initialPosition.x)
                {
                    m_isInStoneState = true;
                    m_isMovingBackToPosition = false;
                }
            }
            else if (m_isInStoneState)
            {
                m_minion.Stone();
            }
            else
            {
                if (m_target == null && !m_isInStoneState)
                {
                    m_patrolTime.Tick(m_minion.time.deltaTime);
                    Patrol();
                }
                else
                { 
                    m_isMovingBackToPosition = false;

                    var targetPos = m_target.position;
                    
                    if (IsLookingAt(targetPos))
                    {
                        var distanceToTarget = Vector2.Distance(m_minion.position, targetPos);
                        if (distanceToTarget <= m_attackRange)
                        {
                            Attack();
                            m_isAttacking = true;
                        }
                        else
                        {
                            MoveTo(targetPos);
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

}
