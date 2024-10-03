using DChild.Gameplay.Projectiles;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BlackDeathSoundMarkProjectile : SimpleAttackProjectile
    {
        [SerializeField]
        private float m_homingDuration;

        private Transform m_target;
        private float m_homingTimer;

        public void HomeTowards(Transform target)
        {
            m_target = target;
            m_homingTimer = m_homingDuration;
        }

        private void Update()
        {
            if (m_homingTimer > 0)
            {
                m_homingTimer -= GameplaySystem.time.deltaTime;
                var toTarget = (m_target.position - transform.position).normalized;
                m_physics.SetVelocity(toTarget * m_physics.velocity.magnitude);
            }
        }
    }

}