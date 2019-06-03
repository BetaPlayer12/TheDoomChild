using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Pathfinding;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class BeeDroneBrain<T> : FlyingMinionAIBrain<T>, IAITargetingBrain where T : BeeDrone
    {
        [SerializeField]
        private float m_chaseRange;

        [SerializeField]
        protected CountdownTimer m_attackRest;

        private T m_beeDrone;
        protected WayPointPatroler m_patrol;
        public abstract void SetTarget(IEnemyTarget target);

        protected abstract void MoveAttackTarget();
        protected bool m_isResting;
        protected bool m_isAttacking;

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(transform.position).destination;
            if (m_navigationTracker.IsCurrentDestination(destination))
            {
                var currentPath = m_navigationTracker.currentPathSegment;
                if (IsLookingAt(currentPath))
                {
                    m_minion.Patrol(currentPath);
                }
                else
                {
                    m_minion.Turn();
                }
            }
            else
            {
                m_navigationTracker.SetDestination(destination);
            }
        }

        private void OnAttackRestEnd(object sender, EventActionArgs eventArgs) => m_isResting = false;

        protected override void Awake()
        {
            base.Awake();
            m_beeDrone = GetComponent<T>();
            m_patrol = GetComponent<WayPointPatroler>();
            m_attackRest.CountdownEnd += OnAttackRestEnd;
            m_patrol.Initialize();
        }

        // Update is called once per frame
        private void Update()
        {
            if (m_beeDrone.waitForBehaviourEnd)
                return;

            if (m_isAttacking)
            {
                m_isAttacking = false;
                m_attackRest.Reset();
                m_isResting = true;
            }
            else if(m_isResting)
            {
                m_attackRest.Tick(m_beeDrone.time.deltaTime);
                m_beeDrone.Idle();
            }
            else if (m_target == null)
            {
                Patrol();
            }
            else
            {
                var targetPos = m_target.position;

                var Distance = Vector2.Distance(m_beeDrone.transform.position, targetPos);

                if (Distance <= m_chaseRange)
                {
                    if (IsLookingAt(targetPos))
                    {
                        MoveAttackTarget();
                    }
                    else
                    {
                        m_beeDrone.Turn();
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