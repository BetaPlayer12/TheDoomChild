using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PossessedHumanBrain : MinionAIBrain<PossessedHuman>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_explodeRange;

        private WayPointPatroler m_patrol;
        private NavigationTracker m_navigationTracker;

        public override void Enable(bool value)
        {
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_minion.Idle();
        }

        public void SetTarget(IEnemyTarget target)
        {
            if(m_target == null)
            {
                m_target = target;
            }
        }

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(transform.position).destination;
            if (m_navigationTracker.IsCurrentDestination(destination))
            {
                var currentPath = m_navigationTracker.currentPathSegment;
                if (IsLookingAt(currentPath))
                {
                    m_minion.MoveTo(currentPath);
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

        private void MoveTo(Vector2 position, Vector2 focus)
        {
            if (m_navigationTracker.IsCurrentDestination(position))
            {
                var currentPath = m_navigationTracker.currentPathSegment;
                if (IsLookingAt(focus))
                {
                    m_minion.MoveTo(currentPath);
                }
                else
                {
                    m_minion.Turn();
                }
            }
            else
            {
                m_navigationTracker.SetDestination(position);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrol = GetComponent<WayPointPatroler>();
            m_navigationTracker = GetComponent<NavigationTracker>();
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if(m_target == null)
            {
                Patrol();
            }
            else
            {
                var distanceToTarget = Vector2.Distance(m_minion.position, m_target.position);
                if (distanceToTarget <= m_explodeRange)
                {
                    m_minion.Explode();
                }
                else
                {
                    var targetPosition = m_target.position;
                    MoveTo(targetPosition, targetPosition);
                }
            }
        }
    }
}