using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MeleeBeeDroneBrain : BeeDroneBrain<MeleeBeeDrone>
    {
        [SerializeField]
        private float m_stingAttackRange;

        [SerializeField]
        private float m_rapidAttackRange;

        public override void Enable(bool value)
        {
            if (!value)
            {
                m_target = null;
            }
            else
            {
                m_minion.Idle();
            }
            enabled = value;
        }

        public override void ResetBrain()
        {
            m_target = null;
            m_attackRest.EndTime(false);
            m_patrol.Initialize();
            m_isResting = false;
            m_isAttacking = false;
        }

        public override void SetTarget(IEnemyTarget target)
        {
            m_target = target;
        }

        protected override void MoveAttackTarget()
        {
            var targetPos = m_target.position;

            var distance = Vector2.Distance(m_minion.position, targetPos);

            if (distance <= m_rapidAttackRange)
            {
                m_minion.RapidSting();
                m_isAttacking = true;
            }
            else if (distance <= m_stingAttackRange)
            {
                m_minion.StingerDive();
                m_isAttacking = true;
            } 
            else
            {
                if (IsLookingAt(targetPos))
                {
                    MoveTo(targetPos);
                }
                else
                {
                    m_minion.Turn();
                }
            }
        }
    }

}