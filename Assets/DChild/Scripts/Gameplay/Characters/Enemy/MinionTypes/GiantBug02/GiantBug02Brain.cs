using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantBug02Brain : MinionAIBrain<GiantBug02>, IAITargetingBrain
    {
        [SerializeField]
        private float m_chaseRange;

        [SerializeField]
        private float m_jumpRange;

        [SerializeField]
        private float m_spitRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        private WayPointPatroler m_patrol;

        private bool m_isAttacking;
        private bool m_isResting;
        private bool m_hasDetected;

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
            m_attackRest.EndTime(false);
            m_isAttacking = false;
            m_isResting = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;

            if(m_target != null && !m_hasDetected)
            {
                if (IsLookingAt(m_target.position))
                {
                    m_minion.DetectPlayer();
                }
                else
                {
                    m_minion.Turn();
                }
                m_hasDetected = true;
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

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_patrol.Initialize(); 
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
                    Patrol();
                    m_hasDetected = false;
                }
                else
                {
                    var targetPos = m_target.position;
                    var distanceToTarget = Vector2.Distance(m_minion.position, targetPos);

                    if(distanceToTarget <= m_chaseRange)
                    {
                        if (IsLookingAt(targetPos))
                        {
                            if (distanceToTarget <= m_spitRange)
                            {
                                m_minion.AcidSpitAttack();
                                m_isAttacking = true;
                            }
                            else
                            {
                                m_minion.JumpAttack();
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
