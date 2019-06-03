using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ZombieBrain : MinionAIBrain<Zombie>
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_attackRange;

        private WayPointPatroler m_patrolHandler;

        public override void Enable(bool value)
        {
            throw new System.NotImplementedException();
        }

        public override void ResetBrain()
        {
            m_minion.Idle();
        }

        private void MoveTo(Vector2 destination, bool usePatrol)
        {
            if (IsNearObstacle())
            {
                ReactToObstacles();
            }
            else
            {
                if (usePatrol)
                {

                    m_minion.PatrolTo(destination);
                }
                else
                {
                    m_minion.MoveTo(destination);
                }
            }
        }

        public bool IsNearObstacle()
        {
            return false;
        }

        private void ReactToObstacles()
        {
            m_minion.Jump();
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandler = GetComponent<WayPointPatroler>();
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_target == null)
            {
                var info = m_patrolHandler.GetInfo(m_minion.position);
                MoveTo(info.destination, true);
            }
            else
            {
                if (IsLookingAt(m_target.position))
                {
                    var distanceToTarget = Vector2.Distance(m_minion.position, m_target.position);
                    if (distanceToTarget <= m_attackRange)
                    {
                        m_minion.UseClaw();
                    }
                    else
                    {
                        MoveTo(m_target.position, false);
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