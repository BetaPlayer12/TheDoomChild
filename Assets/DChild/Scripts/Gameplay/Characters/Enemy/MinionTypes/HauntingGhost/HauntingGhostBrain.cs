using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class HauntingGhostBrain : FlyingMinionAIBrain<HauntingGhost>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_retreatDistance;
        [SerializeField]
        [MinValue(0f)]
        private float m_attackRange;
        [SerializeField]
        [MinValue(0f)]
        private float m_dashDistance;
        [SerializeField]
        [MinValue(0f)]
        private float m_disappearDistance;

        private float m_dashDuration;

#if UNITY_EDITOR
        public float attackRange => m_attackRange;

        public float dashDistance => m_dashDistance;
#endif

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
            m_target = target;
            if (m_minion.position.x < target.position.x)
            {
                m_minion.SetFacing(HorizontalDirection.Right);
            }
            else
            {
                m_minion.SetFacing(HorizontalDirection.Left);
            }
        }

        private bool CanBeSeenBy(IEnemyTarget target)
        {
            if (m_minion.position.x < target.position.x)
            {
                return target.currentFacingDirection == HorizontalDirection.Left;
            }
            else
            {
                return target.currentFacingDirection == HorizontalDirection.Right;
            }
        }

        

        private void MoveAttackTarget(float distanceToTarget)
        {
            if (distanceToTarget > m_attackRange)
            {
                MoveTo(m_target.position);
            }
            else
            {
                m_minion.Dash(m_target,m_dashDuration);
            }
        }

        protected override void MoveTo(Vector2 position)
        {
            m_navigationTracker.SetDestination(position);
            if (m_navigationTracker.pathError)
            {
                m_minion.Stay();
            }
            else
            {
                m_minion.MoveTo(m_navigationTracker.currentPathSegment);
            }
        }

        protected void Start()
        {
            m_dashDuration = m_dashDistance / m_minion.dashSpeed;
        }

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_target != null)
            {
                var distanceToTarget = Vector2.Distance(m_target.position, m_minion.position);
                if (distanceToTarget > m_disappearDistance)
                {
                    m_minion.ForcePool();
                }
                else
                {
                    if (IsLookingAt(m_target.position))
                    {

                        if (CanBeSeenBy(m_target))
                        {
                            var retreatDirection = -GetDirectionToTarget();
                            MoveTo(m_minion.position + (retreatDirection * m_retreatDistance));
                        }
                        else
                        {
                            MoveAttackTarget(distanceToTarget);
                        }
                    }
                    else
                    {
                        m_minion.Despawn();
                    }
                }
            }
        }
    }

}