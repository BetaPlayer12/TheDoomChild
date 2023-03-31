using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class BallisticProjectileLauncher : ProjectileLauncher
    {
        private ProjectileInfo m_projectileInfo;
        private Transform m_spawnPoint;
        private ProjectileLaunchHandle m_handle;
        #region BallistaValues
        private float m_gravityScale;
        private Vector2 m_posOffset = new Vector2(1f, 0.4f);
        private float m_speed;
        private Vector2 m_targetOffset = new Vector2(1.6f, 1);
        #endregion

        private static GameObject m_cacheProjectile;

        public BallisticProjectileLauncher(ProjectileInfo projectileInfo, Transform spawnPoint, float gravityScale, float speed) : base(projectileInfo, spawnPoint)
        {
            this.m_projectileInfo = projectileInfo;
            this.m_spawnPoint = spawnPoint;
            this.m_gravityScale = gravityScale;
            this.m_speed = speed;
            m_handle = new ProjectileLaunchHandle();
        }

        public void LaunchBallisticProjectile(Vector2 target)
        {
            target = new Vector2(target.x, target.y);
            
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectileInfo.projectile);
            instance.transform.position = m_spawnPoint.position;
            var component = instance.GetComponent<Projectile>();
            component.ResetState();
            component.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVelocity(target));
        }

        public void LaunchBallisticProjectile(Vector2 target, GameObject projectile)
        {
            target = new Vector2(target.x, target.y);
            
            projectile.transform.position = m_spawnPoint.position;
            var component = projectile.GetComponent<Projectile>();
            component.ResetState();
            component.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVelocity(target));
        }

        public Vector2 BallisticVelocity(Vector2 target)
        {
            m_projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().simulateGravity = true;
            m_projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;

            var yOffset = target.y + Mathf.Abs(GroundPosition(target).y - m_spawnPoint.position.y);
            target = new Vector2(target.x, Mathf.Abs(target.x - m_spawnPoint.position.x) < 10 ? yOffset : target.y);
            var dir = (target - (Vector2)m_spawnPoint.position);
            //dir = new Vector2(dir.x, dir.y + (Mathf.Abs(target.y - m_spawnPoint.position.y) * 0.5f));
            //Debug.Log("Ballistic Direction " + dir);
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            dir.y = dist;
            dist += h;


            //var currentSpeed = Vector2.Distance(target, m_spawnPoint.position) < 10 ? m_speed : 1;

            var vel = ( Mathf.Sqrt(dist * m_projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale)) /** currentSpeed*/;
            //Debug.Log("Velocity " + vel);
            var ballisticVel = (vel * new Vector2(dir.x * m_posOffset.x, dir.y * m_posOffset.y).normalized) * m_targetOffset.sqrMagnitude; //closest to accurate
            //Debug.Log("Ballistic Velocity " + ballisticVel);
            return ballisticVel;
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

        private Vector2 GroundPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.down, 1000, true, out hitCount, true);
            Debug.DrawRay(startPoint, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        public static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
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
            return m_hitResults;
        }
    }
}
