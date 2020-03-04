using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MummyGuardianBrain : MinionAIBrain<MummyGuardian>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0)]
        private float m_attackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        private CountdownTimer m_chargingDuration;

        [SerializeField]
        private RaySensor m_wallSensor;

        [SerializeField]
        private RaySensor m_groundSensor;

        [SerializeField]
        private RaySensor m_playerSensor;

        private bool m_isAttacking;
        private bool m_isResting;
        private Vector2 targetPos;
        private Vector2 InitialPos;

        private void MoveTo(Vector2 targetPos)
        {
            m_minion.MoveTo(targetPos);
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
            m_chargingDuration.EndTime(false);
            m_isAttacking = false;
            m_isResting = false;
            m_minion.m_isCharging = false;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        private void OnChargeTimeEnd(object sender, EventActionArgs eventArgs) { ChargeReset(); }

        private void ChargeReset()
        {
            m_minion.ChargeAttackPhase3();
            m_wallSensor.enabled = false;
            m_minion.m_isCharging = false;
            m_isResting = true;
            m_attackRest.Reset();
            m_chargingDuration.Reset();
        }

        protected override void Awake()
        {
            base.Awake();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_chargingDuration.CountdownEnd += OnChargeTimeEnd;
            InitialPos = transform.position;
        }

        protected void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            m_wallSensor.Cast();
            m_groundSensor.Cast();

            if (m_isResting)
            {
                m_attackRest.Tick(m_minion.time.deltaTime);
                m_minion.Stay();
            }
            else if (m_minion.m_isCharging)
            {
                m_chargingDuration.Tick(m_minion.time.deltaTime);
               
                m_playerSensor.Cast();

                if (m_playerSensor.isDetecting || m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
                {
                    ChargeReset();
                }
            }
            else if (m_target == null)
            {
                m_minion.Stay();
            }
            else
            {
                var targetPos = m_target.position;

                if(m_groundSensor.isDetecting == false)
                {
                    m_target = null;
                }

                if (m_wallSensor.isDetecting)
                {
                    m_wallSensor.enabled = false;
                    m_minion.Stay();
                }

                m_wallSensor.enabled = true;

                if (m_target != null)
                {
                    if (IsLookingAt(targetPos))
                    {
                        var distance = Vector2.Distance(transform.position, targetPos);
                        if (distance <= m_attackRange)
                        {
                            if (!m_minion.m_isCharging)
                            {
                                m_minion.ChargeAttackPhase1(targetPos);
                            }
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

