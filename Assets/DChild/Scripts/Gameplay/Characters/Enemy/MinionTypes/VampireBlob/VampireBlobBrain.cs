using DChild.Gameplay.Characters.AI;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class VampireBlobBrain : MinionAIBrain<VampireBlob>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(1)]
        private float m_spikeAttackRange;

        [SerializeField]
        private CountdownTimer m_attackRest;

        private WayPointPatroler m_patrol;
        private bool m_isAttacking;
        private bool m_isResting;

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(m_minion.position).destination;
            Debug.Log(destination);
            m_minion.Patrol(destination);
        }

        private void MoveToTarget(Vector2 targetPos)
        {
            m_minion.Move(targetPos);
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
            m_target = null;
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
            m_patrol.Initialize();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
        }

        private void Update()
        {
            Debug.Log(m_target);

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
                if (m_target == null)
                {
                    Patrol();
                }
                else
                {
                    var targetPos = m_target.position;

                    var distanceToTarget = Vector2.Distance(m_minion.position, targetPos);
                    if (distanceToTarget < m_spikeAttackRange)
                    {
                        m_minion.Attack();
                        m_isAttacking = true;
                    }
                    else
                    {
                        MoveToTarget(targetPos);
                    }
                }
            }
        }
    }

}
