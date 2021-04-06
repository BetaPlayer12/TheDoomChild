using DChild;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    [System.Serializable]
    public class ProjectileInfo
    {
        [SerializeField]
        private GameObject m_projectile;
        [SerializeField, MinValue(0.001f)]
        private float m_speed;

        public GameObject projectile => m_projectile;
        public float speed => m_speed;
    }

    [System.Serializable]
    public struct ProjectileLaunchHandle
    {
        public GameObject Launch(GameObject projectile, Vector2 spawnPoint, Vector2 flightDirection, float speed)
        {
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
            instance.transform.position = spawnPoint;
            var component = instance.GetComponent<Projectile>();
            component.ResetState();
            component.SetVelocity(flightDirection, speed);
            return instance.gameObject;
        }

        public GameObject Launch(GameObject projectile, Vector2 flightDirection, float speed)
        {
            var component = projectile.GetComponent<Projectile>();
            component.ResetState();
            component.SetVelocity(flightDirection, speed);

            return projectile.gameObject;
        }
    }

    public class ProjectileLauncher
    {
        private ProjectileInfo m_projectileInfo;
        private Transform m_spawnPoint;
        private ProjectileLaunchHandle m_handle;

        public ProjectileLauncher(ProjectileInfo projectileInfo, Transform spawnPoint)
        {
            this.m_projectileInfo = projectileInfo;
            this.m_spawnPoint = spawnPoint;
            m_handle = new ProjectileLaunchHandle();
        }

        public void SetProjectile(ProjectileInfo info) => m_projectileInfo = info;
        public void SetSpawnPoint(Transform spawnPoint) => m_spawnPoint = spawnPoint;

        public void AimAt(Vector2 target)
        {
            Vector2 spitPos = m_spawnPoint.position;
            Vector3 v_diff = (target - spitPos);
            float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            m_spawnPoint.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
        }

        public void LaunchProjectile() => m_handle.Launch(m_projectileInfo.projectile, m_spawnPoint.position, m_spawnPoint.right, m_projectileInfo.speed);

        public void LaunchProjectile(Vector2 flightDirection) => m_handle.Launch(m_projectileInfo.projectile, m_spawnPoint.position, flightDirection, m_projectileInfo.speed);

        public void LaunchProjectile(Vector2 flightDirection, GameObject projectile) => m_handle.Launch(projectile, flightDirection, m_projectileInfo.speed);
    }
}