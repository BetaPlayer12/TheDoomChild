using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RangeBeeDroneBrain : BeeDroneBrain<RangeBeeDrone>
    {
        [SerializeField]
        private float AttackRange;

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
            var targetPosition = m_target.position;

            if (Vector2.Distance(m_minion.position, m_target.position) <= AttackRange)
            {
                //var direction = m_target.position - m_minion.projectileSpawnPosition;
                //var hit = Physics2D.Raycast(m_minion.projectileSpawnPosition, direction.normalized, AttackRange, Physics2D.GetLayerCollisionMask(gameObject.layer));
                //if (hit.collider.GetComponent<ITarget>() == m_target)
                //{
                //    m_minion.ShootProjectile(m_target);
                //}
                //else
                //{

                //    if (IsLookingAt(targetPosition))
                //    {
                //        MoveTo(targetPosition);
                //    }
                //    else
                //    {
                //        m_minion.Turn();
                //    }
                //}
                //m_minion.StingerDive();
                m_minion.ShootProjectile(m_target);
                m_isAttacking = true;
            }
            else
            {    
                if (IsLookingAt(targetPosition))
                {
                    MoveTo(targetPosition);
                }
                else
                {
                    m_minion.Turn();
                }
            }
        }
    }
}