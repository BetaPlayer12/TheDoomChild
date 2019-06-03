using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SnakeManBrain : MinionAIBrain<SnakeMan>, IAITargetingBrain
    {
        [SerializeField]
        private float m_chaseRange;

        [SerializeField]
        private float m_tailAttackRange;

        [SerializeField]
        private float m_spitAttackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private RaySensor m_groundSensor;

        [SerializeField]
        private RaySensor m_wallSensor;

        private WayPointPatroler m_patrol;

        private bool m_isAttacking;
        private bool m_isResting;
        private bool m_canPatrol=> canPatrol;
        private Vector2 m_guardPost;

        private HorizontalDirection m_facingOnStart;

        public bool canPatrol;

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
            m_patrol?.Initialize();
            m_isAttacking = false;
            m_isResting = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(m_minion.position).destination;

            m_wallSensor.enabled = false;
           
            if (IsLookingAt(destination))
            {
                m_minion.Patrol(destination);
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void MoveTo(Vector2 target)
        {
            if (m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
            {
                m_target = null;
                m_wallSensor.enabled = false;
            }
            else
            {
                m_minion.Move(target);
                m_wallSensor.enabled = true;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
        }

        private void Start()
        {
            m_patrol?.Initialize();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_facingOnStart = m_minion.currentFacingDirection;
            m_guardPost = transform.position;
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
                if (m_canPatrol)
                {
                    Patrol();
                }
                else
                {
                    var distanceToGuardPost = Vector2.Distance(m_guardPost, transform.position);

                    if (distanceToGuardPost <= 2f)
                    {
                        if(m_minion.currentFacingDirection != m_facingOnStart)
                        {
                            m_minion.Turn();
                        }
                        else
                        {
                            m_minion.Stay();
                        }
                    }
                    else
                    {
                        if (IsLookingAt(m_guardPost))
                        {
                            MoveTo(m_guardPost);
                        }
                        else
                        {
                            m_minion.Turn();
                        }
                    }
                }
            }
            else
            {
                var targetPos = m_target.position;
                var distanceToTarget = Vector2.Distance(transform.position, targetPos);

                if(distanceToTarget <= m_chaseRange)
                {
                    if (IsLookingAt(targetPos))
                    {
                        if(distanceToTarget <= m_spitAttackRange)
                        {
                            m_minion.VenomAttack();
                            m_isAttacking = true;
                        }
                        else if (distanceToTarget <= m_tailAttackRange)
                        {
                            m_minion.TailAttack();
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

