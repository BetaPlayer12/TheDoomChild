using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class LeprechaunBrain : MinionAIBrain<Leprechaun>, IAITargetingBrain
    {
        [SerializeField]
        private Territory m_territory;

        [SerializeField]
        private CountdownTimer m_attackRest;

        private BasicAggroSensor m_sensor;

        private bool m_isAttacking;
        private bool m_isResting;
        private bool m_isOnStandBy;

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
            m_isResting = false;
            m_isAttacking = false;
            m_isOnStandBy = true;
        }

        public void SetTarget(IEnemyTarget target)
        {
            if(target != null)
            {
                m_target = target;
            }
            else
            {
                m_target = null;
            }

           if(m_isOnStandBy && m_target!= null)
            {
                m_minion.Detect();
                m_isOnStandBy = false;
            }
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_sensor = GetComponentInChildren<BasicAggroSensor>();
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
                m_sensor.enabled = true;
                m_minion.Stay();
                m_isOnStandBy = true;
            }
            else
            {
                var targetPos = m_target.position;
                m_sensor.enabled = false;
                m_isOnStandBy = false;

                if (m_territory.Contains(targetPos))
                {
                    if (IsLookingAt(targetPos))
                    {
                        m_minion.SummonGoldAttack(targetPos);
                        m_isAttacking = true;
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
