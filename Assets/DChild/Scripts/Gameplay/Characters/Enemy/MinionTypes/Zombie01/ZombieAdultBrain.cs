using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ZombieAdultBrain : MinionAIBrain<ZombieAdult>, IAITargetingBrain
    {
        [SerializeField]
        private float m_attackRange;

        [SerializeField]
        private float m_chaseRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private RaySensor m_groundSensor;

        [SerializeField]
        private RaySensor m_wallSensor;

        private WayPointPatroler m_patrol;

        private bool m_isAttacking;
        private bool m_isResting;
        private bool m_isOnStandBy;
        private bool m_toReach;

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

        private void MoveTo(Vector2 targetPos)
        {
            m_wallSensor.Cast();
            m_groundSensor.Cast();

            if (m_wallSensor.isDetecting)
            {
                m_toReach = true;
                m_wallSensor.enabled = false;
            }
            else if (m_groundSensor.isDetecting == false)
            {
                m_minion.Stay();
                m_wallSensor.enabled = false;
            }
            else
            {
                m_minion.MoveTo(targetPos);
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
                m_minion.Stay();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {

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

        private void Attack()
        {
            var rand = Mathf.Abs(Random.Range(1, 3));

            switch (rand)
            {
                case 1:
                    m_minion.ScratchAttack();
                    break;
                case 2:
                    m_minion.BiteAttack();
                    break;
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
                Patrol();
            }
            else
            {
                var targetPos = m_target.position;

                var distance = Vector2.Distance(m_minion.transform.position, targetPos);

                if(distance <= m_chaseRange)
                {
                    if (IsLookingAt(targetPos))
                    {
                        if (distance <= m_attackRange)
                        {
                            if(m_toReach && targetPos.y > m_wallSensor.transform.position.y)
                            {
                                m_minion.ScratchJumpAttack();
                            }
                            else
                            {
                                Attack();
                            }
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
                        m_toReach = false;
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
