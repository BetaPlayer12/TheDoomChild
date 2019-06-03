using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class CactusBugBrain : MinionAIBrain<CactusBug>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0)]
        private float m_chaseRange;

        [SerializeField]
        [MinValue(0)]
        private float m_spitAttackRange;

        [SerializeField]
        [MinValue(0)]
        private float m_jumpAttackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        [TabGroup("Reference")]
        private RaySensor m_groundSensor;

        [SerializeField]
        [TabGroup("Reference")]
        private RaySensor m_wallSensor;

        private bool m_isAttacking;
        private bool m_isResting;
        private bool m_hasDetectedPlayer;

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

        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;

            if (m_target != null && m_hasDetectedPlayer == false)
            {
                m_minion.BurrowReveal();
                m_hasDetectedPlayer = true;
            }
        }

        private void MoveTo(Vector2 target)
        {
            if (m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
            {
                m_minion.Idle();
                m_wallSensor.enabled = false;
            }
            else
            {
                m_minion.Move();
                m_wallSensor.enabled = true;
            }
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        private void Start()
        {
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_minion.BurrowIdle();
        }

        protected override void Awake()
        {
            base.Awake();
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
            else
            {
                if (m_target == null)
                {
                    if (!m_hasDetectedPlayer)
                    {
                        m_minion.BurrowIdle();
                    }
                    else
                    {
                        m_minion.Burrow();
                        m_hasDetectedPlayer = false;
                    }                
                }
                else
                {
                    var targetPos = m_target.position;
                    var distanceToTarget = Vector2.Distance(transform.position, targetPos);

                    if (distanceToTarget <= m_chaseRange)
                    {
                        if (IsLookingAt(targetPos))
                        {
                            if (distanceToTarget <= m_spitAttackRange)
                            {
                                m_minion.SpitAttack();
                                m_isAttacking = true;
                            }
                            else if(distanceToTarget <= m_jumpAttackRange)
                            {
                                m_minion.JumpAttack();
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
