using DChild;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters
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

    [System.Serializable]
    public struct ProjectileLauncher
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
}