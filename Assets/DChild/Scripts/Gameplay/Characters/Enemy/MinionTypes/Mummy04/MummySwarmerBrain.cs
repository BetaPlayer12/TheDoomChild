using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MummySwarmerBrain : MinionAIBrain<MummySwarmer>, IAITargetingBrain
    {
        [SerializeField]
        private Territory m_territory;

        [SerializeField]
        private RaySensor m_groundSensor;

        [SerializeField]
        private RaySensor m_wallSensor;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private float m_attackRange;

        private WayPointPatroler m_patrol;
        private bool m_isAttacking;
        private bool m_isResting;

        public void Patrol()
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

        public void MoveTo(Vector2 position)
        {
            m_minion.Move(position);
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
            m_patrol = GetComponent<WayPointPatroler>();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            m_wallSensor.Cast();
            m_groundSensor.Cast();

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
                    m_wallSensor.enabled = false;
                }
                else
                {
                    var targetPos = m_target.position;
                    m_wallSensor.enabled = true;
                    Debug.Log("Detecting: " + m_groundSensor.isDetecting);
                    if (m_territory.Contains(targetPos) && m_groundSensor.isDetecting && !m_wallSensor.isDetecting)
                    {
                        if (IsLookingAt(targetPos))
                        {
                            var distanceToTarget = Vector2.Distance(m_minion.position, targetPos);
                            if (distanceToTarget <= m_attackRange)
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
