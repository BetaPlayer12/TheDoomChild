using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Environment;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Scorpion01Brain : MinionAIBrain<Scorpion01>, IAITargetingBrain
    {
        [SerializeField]
        private Territory m_territory;

        [SerializeField]
        private CountdownTimer m_attackRest;

        [SerializeField]
        [MinValue(0f)]
        private float m_attackRange;

        private WayPointPatroler m_patrol;

        private bool m_isResting;
        private bool m_isAttacking;

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
        }

        public void SetTarget(IEnemyTarget target)
        {
            if (m_target == null)
            {
                m_target = target;
            }
        }

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

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_patrol.Initialize();

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
                Patrol();
            }
            else
            {
                var targetPosition = m_target.position;
                if (m_territory.Contains(targetPosition))
                {
                    if (IsLookingAt(targetPosition))
                    {
                        if (Vector2.Distance(m_minion.position, targetPosition) <= m_attackRange)
                        {
                            m_minion.TailAttack();
                            m_isAttacking = true;
                        }
                        else
                        {
                            m_minion.MoveTo(targetPosition);
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
