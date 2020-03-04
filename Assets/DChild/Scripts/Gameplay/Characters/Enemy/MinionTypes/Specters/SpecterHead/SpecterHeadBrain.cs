using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SpecterHeadBrain : SpecterBrain<SpecterHead>, IAITargetingBrain
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_dashRange;
        [SerializeField]
        [MinValue(0f)]
        private float m_dashDistance;

        private float m_dashDuration;

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

        protected override void MoveAttackTarget()
        {
            var targetPosition = m_target.position;
            if (IsLookingAt(targetPosition))
            {
                if (Vector2.Distance(m_minion.position, targetPosition) <= m_dashRange)
                {
                    m_minion.Dash(m_target, m_dashDuration);
                }
                else
                {
                    MoveTo(targetPosition);
                }
            }
            else
            {
                m_minion.Turn();
            }
        }

        private void Start()
        {
            m_dashDuration = m_dashDistance / m_minion.dashSpeed;
        }
    }

}