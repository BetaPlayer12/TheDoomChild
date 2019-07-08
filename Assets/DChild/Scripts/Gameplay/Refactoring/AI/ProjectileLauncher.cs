using DChild;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters
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
        public void Launch(GameObject projectile, Vector2 spawnPoint, Vector2 flightDirection, float speed)
        {
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
            instance.transform.position = spawnPoint;
            var component = instance.GetComponent<Projectile>();
            component.ResetState();
            component.SetVelocity(flightDirection, speed);
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
        public void LaunchProjectile()
        {
            m_handle.Launch(m_projectileInfo.projectile, m_spawnPoint.position, m_spawnPoint.right, m_projectileInfo.speed);
        }
    }
}