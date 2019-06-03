using DChild.Gameplay.Characters.AI;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using UnityEngine;
using Holysoft.Event;
using Holysoft.Collections;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SlugBrain : MinionAIBrain<Slug>, IAITargetingBrain
    {
        [SerializeField]
        private float m_chaseRange;
        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        [TabGroup("Reference")]
        private RaySensor m_wallSensor;
        [SerializeField]
        [TabGroup("Reference")]
        private RaySensor m_groundSensor;

        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_spitRange;
        [SerializeField]
        [TabGroup("Ability Ranges")]
        private float m_spikeRange;

        private WayPointPatroler m_patrol;

        private bool m_isAttacking;
        private bool m_isResting;

        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
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

        public float GetDistance(Vector2 targetPos)
        {
            return Vector2.Distance(targetPos, transform.position);
        }

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
                m_minion.Move();
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void Move(Vector2 targetPos)
        {
            if (m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
            {
                m_target = null;
                m_minion.Scout();
                m_wallSensor.enabled = false;
            }
            else
            {
                m_minion.Move();
                m_wallSensor.enabled = true;
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

        void Update()
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
            else
            {
                if(m_target == null)
                {
                    Patrol();
                }
                else
                {
                    var targetPos = m_target.position;

                    if(GetDistance(targetPos) < m_chaseRange)
                    {
                        if (IsLookingAt(targetPos))
                        {
                            if (GetDistance(targetPos) < m_spitRange)
                            {
                                m_minion.Spit(targetPos);
                                m_isAttacking = true;
                            }
                            else if (GetDistance(targetPos) < m_spikeRange) //Not yet done
                            {
                                m_minion.SpikeProjectiles();
                                m_isAttacking = true;
                            }
                            else
                            {
                                Move(targetPos);
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
                        m_minion.Scout();
                    }
                }
            }
        }
    }
}