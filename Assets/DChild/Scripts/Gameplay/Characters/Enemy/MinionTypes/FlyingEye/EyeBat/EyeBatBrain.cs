using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class EyeBatBrain : FlyingEyeBrain<EyeBat>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_attackRange;

        public override void Enable(bool value)
        {
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_target = null;
        }

        public void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        protected void MoveTo(Vector2 position, Vector2 focus)
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

        private void Update()
        {
            if (m_minion.waitForBehaviourEnd)
                return;
            if (m_target == null)
            {
                Patrol();
            }
            else
            {
                var targetPosition = m_target.position;
                if (Vector2.Distance(m_minion.position, targetPosition) <= m_attackRange)
                {
                    if (IsLookingAt(targetPosition))
                    {
                        m_minion.Lunge();
                    }
                    else
                    {
                        m_minion.Turn();
                    }
                }
                else
                {
                    MoveTo(targetPosition, targetPosition);
                }
            }
        }
    }

}