using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardDeathStenchProjectile : SimpleAttackProjectile
    {
        [SerializeField]
        private float m_homingDuration;
        [SerializeField]
        private float m_homingEarlyCancelDistance;

        private Vector2 m_flightDirection;

        [Button]
        public void DoSomething(Transform target, float speed)
        {
            m_physics.SetVelocity(Vector2.zero);
            StopAllCoroutines();
            StartCoroutine(HomeInTowards(target, speed));
        }

        private IEnumerator HomeInTowards(Transform target, float speed)
        {
            var time = 0f;
            var homingTimer = m_homingDuration;
            var distanceToTarget = 0f;
            do
            {
                time += GameplaySystem.time.fixedDeltaTime;

                var toTargetVector = target.position - transform.position;

                distanceToTarget = toTargetVector.magnitude;
                m_flightDirection = toTargetVector.normalized;
                m_physics.SetVelocity(m_flightDirection * speed);
                homingTimer -= GameplaySystem.time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            } while (homingTimer > 0 && distanceToTarget > m_homingEarlyCancelDistance);
        }
    }
}