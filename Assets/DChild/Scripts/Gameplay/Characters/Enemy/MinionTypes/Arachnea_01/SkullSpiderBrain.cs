using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkullSpiderBrain : MinionAIBrain<SkullSpider>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0)]
        private float m_attackRange;

        [SerializeField]
        [MinValue(0)]
        private float m_chaseRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private RaySensor m_wallSensor;

        [SerializeField]
        private RaySensor m_groundSensor;

        private WayPointPatroler m_patrol;

        private bool m_isResting;
        private bool m_isAttacking;

        private void Patrol()
        {
            m_wallSensor.enabled = true;

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

        private void MoveTo(Vector2 targetPos)
        {
            m_wallSensor.Cast();
            m_groundSensor.Cast();

            if(m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
            {
                m_target = null;
                m_wallSensor.enabled = false;
            }
            else
            {
                m_minion.MoveTo(targetPos);
            }
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
            throw new System.NotImplementedException();
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
        }
        protected void Update()
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
                if(m_target == null)
                {
                    Patrol();
                }
                else
                {
                    var targetPos = m_target.position;

                    if (IsLookingAt(targetPos))
                    {
                        var distance = Vector2.Distance(m_minion.position, targetPos);

                        if(distance <= m_chaseRange)
                        {
                            if(distance <= m_attackRange)
                            {
                                m_minion.Attack();
                                m_isAttacking = true;
                            }
                            else
                            {
                                MoveTo(targetPos);
                            }
                        }
                        else
                        {
                            m_target = null;
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
