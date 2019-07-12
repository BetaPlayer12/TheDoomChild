using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MushroomManBrain : MinionAIBrain<MushroomMan>, IAITargetingBrain
    {
        [SerializeField]
        private float m_chaseRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private float m_attackRange;

        [SerializeField]
        private RaySensor m_wallSensor;

        [SerializeField]
        private RaySensor m_groundSensor;

        private float NormalYThreshold = 2f;

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

        public bool FacingDestination(Vector2 destination)
        {
            Vector2 pos = transform.position;
            if (destination.x > pos.x && transform.localScale.x == -1 || destination.x < pos.x && transform.localScale.x == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Patrol()
        {
            Vector2 pos = transform.position;
            var destination = m_patrol.GetInfo(pos).destination;
            if (pos != destination)
            {
                //var currentPath = m_navigationTracker.currentPathSegment;
                if (FacingDestination(destination))
                {
                    //Debug.Log("Do Patrol Routine");
                    m_minion.Patrol();
                }
                else
                {
                    //Debug.Log("Do Patrol Turn");
                    m_minion.Turn();
                }
            }
        }

        private void Turn()
        {
            if (m_target.position.x < transform.position.x && transform.localScale.x == -1)
            {
                //m_facingPlayer = false;
                m_minion.Turn();
            }
            else if (m_target.position.x > transform.position.x && transform.localScale.x == 1)
            {
                //m_facingPlayer = false;
                m_minion.Turn();
            }
            //else
            //{
            //    m_facingPlayer = true;
            //}
        }

        private void MoveTo(Vector2 targetPos)
        {
            var direction = targetPos - m_minion.position;
            var directionNormal = direction.normalized;

            if (m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
            {
                m_target = null;
                m_minion.Stay();
                m_wallSensor.enabled = false;
            }
            else if (Mathf.Abs(directionNormal.y) < NormalYThreshold)
            {
                m_minion.Stay();
            }
            else
            {
                m_minion.MoveTo();
                m_wallSensor.enabled = true;
            }
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_patrol = GetComponent<WayPointPatroler>();
            //m_patrol.Initialize();
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
                if(m_target == null)
                {
                    Patrol();
                }
                else
                {
                    var targetPos = m_target.position;
                    var distanceToTarget = Vector2.Distance(m_minion.position, targetPos);

                    if (distanceToTarget <= m_chaseRange)
                    {
                        if (!IsLookingAt(targetPos))
                        {
                            
                            if (distanceToTarget <= m_attackRange)
                            {
                                m_minion.Attack();
                                m_isAttacking = true;
                            }
                            else
                            {
                                m_minion.MoveTo();
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
