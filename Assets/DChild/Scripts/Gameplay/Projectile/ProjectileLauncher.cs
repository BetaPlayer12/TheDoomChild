using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Projectiles.Handlers
{
    [SerializeField]
    public class ProjectileInfo
    {
        [SerializeField]
        private GameObject m_projectile;
        [SerializeField, MinValue(0.001f)]
        private float m_speed;

        public GameObject projectile => m_projectile;
        public float speed => m_speed;
    }

    public struct ProjectileLauncher
    {
        public AttackProjectile FireProjectileTo(GameObject projectile, Scene toPlace, Vector2 spawnPoint, Vector2 target, float speed)
        {
            var instance = GetOrCreateProjectile<AttackProjectile>(projectile, toPlace);
            instance.transform.position = spawnPoint;
            var direction = (target - spawnPoint).normalized;
            instance.SetVelocity(direction, speed);
            return instance;
        }

        public static T GetOrCreateProjectile<T>(GameObject projectile, Scene toPlace) where T : AttackProjectile
        {
            T toCreate = projectile.GetComponent<T>();
            if (toCreate == null)
            {
                return null;
            }
            else
            {
                var pooledProjectile = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                pooledProjectile.transform.parent = null;
                SceneManager.MoveGameObjectToScene(pooledProjectile.gameObject, toPlace);
                return (T)pooledProjectile;
            }
        }
    }
}
