using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RangeBeeDrone : BeeDrone
    {
        private RangeBeeDroneAnimation m_anim;
        protected override BeeDroneAnimation m_animation => m_anim;

        [SerializeField]
        private GameObject m_toxicProjectile;
        [SerializeField]
        private Transform m_projectileSpawn;

        public Vector2 projectileSpawnPosition => m_projectileSpawn.position;

        public void StingerDive()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(StingerDiveRoutine()));
        }

        public void ShootProjectile(ITarget target)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            
            m_behaviour.SetActiveBehaviour(StartCoroutine(ShootProjectileRoutine(target)));
        }

        private IEnumerator StingerDiveRoutine()
        {
            m_waitForBehaviourEnd = true;
            EnableRoot(true, true, false);
            m_movement.Stop();
            m_anim.DoStingerDive();
            yield return new WaitForAnimationComplete(animation.animationState, RangeBeeDroneAnimation.ANIMATION_STINGERDIVE);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);

        }

        private IEnumerator ShootProjectileRoutine(ITarget target)
        {
            m_waitForBehaviourEnd = true;
            EnableRoot(true, true, false);
            m_movement.Stop();
            m_anim.DoToxicShot();
            yield return new WaitForAnimationEvent(animation.animationState, RangeBeeDroneAnimation.EVENT_PROJECTILE);
            var projectile = (ProjectileSting)GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_toxicProjectile); 
            var direction = (target.position - (Vector2)m_projectileSpawn.position);
            projectile.ChangeTrajectory(direction.normalized);
            projectile.SetVelocity(direction, 3f);
            yield return new WaitForAnimationComplete(animation.animationState, RangeBeeDroneAnimation.ANIMATION_TOXICSHOT);
            m_waitForBehaviourEnd = false;
            StopActiveBehaviour();
        }

        protected override void Awake()
        {
            base.Awake();
            m_anim = GetComponent<RangeBeeDroneAnimation>();
        }
    }
}