using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class TeleportingSkull : AttackBehaviour
    {
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Projectile")]
        private ProjectileInfo m_projectile;
        public ProjectileInfo projectile => m_projectile;
        private Projectile m_spawnedProjectile;
        public Projectile spawnedProjectile => m_spawnedProjectile;
        private bool m_canTeleport;
        public bool canTeleport => m_canTeleport;

        private void DisableTeleport(object sender, EventActionArgs eventArgs)
        {
            m_canTeleport = false;
            m_spawnedProjectile = null;
        }

        public void GetSpawnedProjectile(Projectile spawnedProjectile)
        {
            if (m_spawnedProjectile == null && m_canTeleport)
            {
                m_spawnedProjectile = spawnedProjectile;
                m_spawnedProjectile.Impacted += DisableTeleport;
            }
        }

        public void TeleportToProjectile()
        {
            m_canTeleport = false;
            m_character.transform.position = Mathf.Abs(RoofPosition(m_spawnedProjectile.transform.position).y - m_spawnedProjectile.transform.position.y) < 5f ? new Vector3(m_spawnedProjectile.transform.position.x, m_spawnedProjectile.transform.position.y - m_character.height) : m_spawnedProjectile.transform.position;
            m_spawnedProjectile.CallPoolRequest();
            base.AttackOver();
        }

        public void Execute()
        {
            m_canTeleport = true;
        }

        protected Vector2 RoofPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.up, 100f, true, out hitCount, true);
            if (hit != null)
            {
                Debug.DrawRay(startPoint, hit[0].point);
                return hit[0].point;
            }
            return Vector2.zero;
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        protected static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                }
            }
#endif
            return hitCount == 0 ? null : m_hitResults;
        }
    }
}
