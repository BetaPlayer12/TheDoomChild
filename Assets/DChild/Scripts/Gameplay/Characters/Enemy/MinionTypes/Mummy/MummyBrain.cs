using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class MummyBrain<T> : MinionAIBrain<T>, IAITargetingBrain where T: Mummy
    {
        [SerializeField]
        protected RaySensor m_wallSensor;

        [SerializeField]
        protected RaySensor m_groundSensor;

        private T m_mummy;
        protected abstract float attackRange { get; }       
        protected abstract CountdownTimer attackRest { get; }

        protected bool m_isAttacking;
        protected bool m_isResting;

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        protected abstract void Attack();

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        private void MoveTo(Vector2 targetPos)
        {
            m_wallSensor.Cast();
            m_groundSensor.Cast();

            if(m_wallSensor.isDetecting || m_groundSensor.isDetecting == false)
            {
                m_mummy.Stay();
                m_wallSensor.enabled = false;
            }
            else
            {
                m_minion.MoveTo(targetPos);
                m_wallSensor.enabled = true;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            m_mummy = GetComponent<T>();           
            attackRest.CountdownEnd += OnAttackRestEnd;
        }

        protected void Update()
        {
            if (m_mummy.waitForBehaviourEnd)
                return;

            if (m_isAttacking)
            {
                m_isAttacking = false;
                attackRest.Reset();
                m_isResting = true;
            }
            else if (m_isResting)
            {
                attackRest.Tick(m_minion.time.deltaTime);
                m_mummy.Stay();
            }
            else if (m_target == null)
            {
                m_mummy.Stay();
            }
            else
            {
                var targetPos = m_target.position;

                if (IsLookingAt(targetPos))
                {
                    var distance = Vector2.Distance(transform.position, targetPos);
                    if (distance <= attackRange)
                    {
                        Attack();
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
        }
    }
}
