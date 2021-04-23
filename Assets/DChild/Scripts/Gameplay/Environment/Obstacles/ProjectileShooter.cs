using DChild.Gameplay.Characters;
using DChild.Gameplay.Projectiles;
using Holysoft.Pooling;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class ProjectileShooter : MonoBehaviour
    {
        [System.Serializable]
        private class AnimatedInfo
        {
            [SerializeField, Spine.Unity.SpineAnimation]
            private string m_fireAnimation;
            [SerializeField, Spine.Unity.SpineAnimation]
            private string m_addedAnimation;
            [SerializeField, SpineEvent]
            private string m_fireEvent;

            public string fireAnimation => m_fireAnimation;
            public string addedAnimation => m_addedAnimation;
            public string fireEvent => m_fireEvent;
        }

        private enum FireMethod
        {
            Basic,
            DelayedBasic,
            Animated
        }

        [SerializeField]
        private ProjectileInfo m_toShoot;
        [SerializeField]
        private Transform m_spawnPoint;
        [SerializeField]
        private float m_offset;
        [SerializeField]
        private GameObject m_shootFX;
        [SerializeField]
        private FireMethod m_fireMethod;

        [SerializeField, MinValue(0), ShowIf("@m_fireMethod == FireMethod.DelayedBasic")]
        private float m_fireDelay;
        [SerializeField, ShowIf("@m_fireMethod == FireMethod.DelayedBasic")]
        private GameObject m_preShootFX;
        [SerializeField, ShowIf("@m_fireMethod == FireMethod.Animated")]
        private AnimatedInfo m_animationInfo;

        [SerializeField]
        private Collider2D[] m_ignoreColliders;

        private SpineAnimation m_animation;
        private Coroutine m_routine;
        private ProjectileLaunchHandle m_launcher;
        private FXSpawnHandle<FX> m_FXSpawner;

        public float fireDelay => m_fireDelay;

        [Button]
        public void Shoot()
        {
            switch (m_fireMethod)
            {
                case FireMethod.Basic:
                    SpawnProjectile();
                    break;
                case FireMethod.DelayedBasic:
                    if (m_routine == null)
                    {
                        m_routine = StartCoroutine(DelayShootRoutine());
                    }
                    break;
                case FireMethod.Animated:
                    if (m_routine == null)
                    {
                        m_routine = StartCoroutine(AnimatedShootRoutine());
                    }
                    break;
            }
        }

        private IEnumerator DelayShootRoutine()
        {
            if (m_fireDelay > 0)
            {
                if (m_preShootFX == null)
                {
                    yield return new WaitForSeconds(m_fireDelay);
                }
                else
                {
                    var preShootFX = m_FXSpawner.InstantiateFX(m_preShootFX, m_spawnPoint.position);
                    preShootFX.transform.parent = m_spawnPoint;
                    yield return new WaitForSeconds(m_fireDelay);
                    preShootFX.Stop();
                }
            }
            SpawnProjectile();
            m_routine = null;
        }

        private IEnumerator AnimatedShootRoutine()
        {
            m_animation.SetAnimation(0, m_animationInfo.fireAnimation, false);
            m_animation.AddAnimation(0, m_animationInfo.addedAnimation, true, 0f);
            yield return new WaitForAnimationEvent(m_animation.animationState, m_animationInfo.fireEvent);
            SpawnProjectile();
            m_routine = null;
        }

        private void SpawnProjectile()
        {
            var position = m_spawnPoint.position + m_spawnPoint.right * m_offset;
            var instance = m_launcher.Launch(m_toShoot.projectile, position, m_spawnPoint.right, m_toShoot.speed);
            if (m_shootFX != null)
            {
                m_FXSpawner.InstantiateFX(m_shootFX, m_spawnPoint.position);
            }

            if (m_ignoreColliders.Length > 0)
            {
                var colliders = instance.GetComponentsInChildren<Collider2D>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    for (int j = 0; j < m_ignoreColliders.Length; j++)
                    {
                        Physics2D.IgnoreCollision(colliders[i], m_ignoreColliders[j], true);
                    }
                }
            }
            instance.GetComponentInChildren<Projectile>().PoolRequest += OnPool;
        }

        private void OnPool(object sender, PoolItemEventArgs eventArgs)
        {
            var colliders = ((Projectile)sender).GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                for (int j = 0; j < m_ignoreColliders.Length; j++)
                {
                    Physics2D.IgnoreCollision(colliders[i], m_ignoreColliders[j], false);
                }
            }
        }

        private void Awake()
        {
            if (m_fireMethod == FireMethod.Animated)
            {
                m_animation = GetComponentInChildren<SpineAnimation>();
                m_fireDelay = m_animation.skeletonAnimation.Skeleton.Data.FindAnimation(m_animationInfo.fireAnimation).Duration;
            }
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = true;
            if (m_toShoot.projectile != null)
            {
                filter.SetLayerMask(Physics2D.GetLayerCollisionMask(m_toShoot.projectile.layer));
            }
            else
            {
                filter.SetLayerMask(LayerMask.GetMask("Environment"));
            }
            filter.useTriggers = false;
            RaycastHit2D[] hitbuffers = new RaycastHit2D[16];
            var position = m_spawnPoint.position + m_spawnPoint.right * m_offset;
            var hits = Physics2D.Raycast(position, m_spawnPoint.right, filter, hitbuffers);
            Gizmos.color = Color.red;
            if (hits > 0)
            {
                RaycastHit2D hit = hitbuffers[0];
                var toHit = hit.point - (Vector2)position;
                Gizmos.DrawRay(position, toHit);
            }
            else
            {
                Gizmos.DrawRay(position, m_spawnPoint.right * 1000f);
            }
#endif
        }

        private void OnValidate()
        {
            if (m_spawnPoint == null)
            {
                m_spawnPoint = transform;
            }

            switch (m_fireMethod)
            {
                case FireMethod.Basic:
                    m_fireDelay = 0f;
                    m_animationInfo = null;
                    break;
                case FireMethod.DelayedBasic:
                    m_animationInfo = null;
                    break;
                case FireMethod.Animated:
                    if (m_animationInfo == null)
                    {
                        m_animationInfo = new AnimatedInfo();
                    }
                    break;
            }
        }
    }
}
