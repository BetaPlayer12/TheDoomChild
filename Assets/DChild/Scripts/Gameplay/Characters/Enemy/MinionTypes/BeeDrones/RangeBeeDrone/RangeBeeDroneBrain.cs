using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RangeBeeDroneBrain : BeeDroneBrain<RangeBeeDrone>
    {
        [SerializeField]
        private float AttackRange;
        [SerializeField]
        private float ToxicShotRange;

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

            var distance = Vector2.Distance(transform.position, m_target.position);
            //needs to set up the the PID of "toxicshot" prefab.
            //if (distance <= ToxicShotRange)
            //{
            //    var direction = m_target.position - m_minion.projectileSpawnPosition;
            //    var hit = Physics2D.Raycast(m_minion.projectileSpawnPosition, direction.normalized, AttackRange, Physics2D.GetLayerCollisionMask(gameObject.layer));
            //    if (hit.collider.GetComponent<ITarget>() == m_target) //cannot see player.
            //    {
            //        //m_minion.ShootProjectile(m_target);
            //    }
            //}
            //else
            if (distance <= AttackRange)
            {
                m_minion.StingerDive();
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