using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ShellBugBrain : MinionAIBrain<ShellBug>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0)]
        private float m_chaseRange;

        [SerializeField]
        [MinValue(0)]
        private float m_attackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private RaySensor m_wallSensor;

        [SerializeField]
        private RaySensor m_groundSensor;

        private float NormalYThreshold = 2f;

        private WayPointPatroler m_patrol;

        private bool m_isAttacking;
        private bool m_isResting;


        private void Patrol()
        {
            var destination = m_patrol.GetInfo(m_minion.position).destination;

            m_wallSensor.enabled = false;

            if (m_patrol.isNearDestination)
            {
                m_minion.Scout();
            }
            else if (IsLookingAt(destination))
            {
                m_minion.Patrol();
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void MoveTo(Vector2 targetPos)
        {
            var direction = targetPos - m_minion.position;
            var directionNormal = direction.normalized;

            if (m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
            {
                m_target = null;
                m_minion.Scout();
                m_wallSensor.enabled = false;
            }
            else if (Mathf.Abs(directionNormal.y) < NormalYThreshold)
            {
                m_minion.Idle();
            }
            else
            {
                m_minion.MoveTo();
                m_wallSensor.enabled = true;
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
                m_minion.Idle();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_minion.Idle();
            m_target = null;
            m_patrol.Initialize();
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
            m_patrol = GetComponent<WayPointPatroler>();
            m_patrol.Initialize();
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
                m_minion.Idle();
            }
            else if (m_target == null)
            {
                Patrol();
            }
            else
            {
                m_wallSensor.Cast();
                m_groundSensor.Cast();

                var targetPos = m_target.position;
                var distanceToTarget = Vector2.Distance(targetPos, m_minion.transform.position);

                if (distanceToTarget <= m_chaseRange)
                {
                    if (IsLookingAt(targetPos))
                    {
                        if (distanceToTarget <= m_attackRange)
                        {
                            m_minion.Charge(targetPos);
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
                else
                {
                    m_target = null;
                }
            }
        }
    }
}
