using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pathfinding;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GiantBeetleBrain : MinionAIBrain<GiantBeetle>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_activityRadius;

        [SerializeField]
        [MinValue(0f)]
        private float m_attackDistance;

        private WayPointPatroler m_patrol;
        private bool m_hasAttacked;

        private void Patrol()
        {
            var destination = m_patrol.GetInfo(transform.position).destination;
            m_minion.Patrol(destination);
        }

        private bool CanHitTarget()
        {
            var attackDirection = (m_minion.currentFacingDirection == HorizontalDirection.Left ? Vector2.left : Vector2.right);
            var hit = Physics2D.Raycast(transform.position, attackDirection, m_attackDistance, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawRay(transform.position, attackDirection * m_attackDistance);
            return hit.collider != null;
        }
        
        private void MoveTo(Vector2 destination)
        {
            m_minion.MoveToDestination(destination);
        }

        public override void Enable(bool value)
        {
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_target = null;
            m_patrol.Initialize();
            m_hasAttacked = false;
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

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;

            if (m_target != null)
            {
                if (CanHitTarget())
                {
                    MoveTo(m_target.position);
                }
               Patrol();
            }
        }

        protected override void Awake()
        {
            m_patrol = GetComponent<WayPointPatroler>();
            base.Awake();
        }

#if UNITY_EDITOR
        public float activityRadius => m_activityRadius;
        public float attackDistance => m_attackDistance;
#endif
    }
}
