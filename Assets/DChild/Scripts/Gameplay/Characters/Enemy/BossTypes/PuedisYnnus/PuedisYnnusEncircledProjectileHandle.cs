using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using DChild.Gameplay.Projectiles;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusEncircledProjectileHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_projectile;
        [SerializeField, MinValue(1)]
        private int m_projectileCount = 1;
        [SerializeField]
        private float m_distanceFromCenter;
        [SerializeField]
        private float m_rotationSpeed;

        private List<Projectile> m_spawnedProjectiles;
        private RotateObject m_objectRotator;

        public void SpawnProjectiles()
        {
            var rotationInterval = 360f / m_projectileCount;
            for (int i = 0; i < m_projectileCount; i++)
            {
                var rotation = rotationInterval * i;
                var RadRotation = Mathf.Deg2Rad * rotation;
                var angleInVector2D = new Vector3(Mathf.Cos(RadRotation), Mathf.Sin(RadRotation), 0);
                var position = transform.position + (m_distanceFromCenter * angleInVector2D);
                var angle = Quaternion.Euler(Vector3.forward * rotation);
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectile, position, angle);
                instance.transform.parent = transform;

                m_spawnedProjectiles.Add(instance);
            }
        }

        public void ScatterProjectiles(float speed)
        {
            for (int i = 0; i < m_spawnedProjectiles.Count; i++)
            {
                var instance = m_spawnedProjectiles[i];
                if (instance == null)
                    continue;

                instance.transform.parent = null;
                instance.GetComponent<Rigidbody2D>().velocity = instance.transform.right * speed;
            }
            m_spawnedProjectiles.Clear();
        }

        public void StartRotation()
        {
            var rotationDirectionModifer = Random.Range(0, 100) > 50 ? 1 : -1;
            m_objectRotator.SetSpeed(new Vector3(0, 0, m_rotationSpeed * rotationDirectionModifer));
        }

        public void StopRotation()
        {
            m_objectRotator.SetSpeed(Vector3.zero);
        }

        private void Awake()
        {
            m_spawnedProjectiles = new List<Projectile>();
            m_objectRotator = GetComponent<RotateObject>();
        }

        private void OnDrawGizmosSelected()
        {
            var rotationInterval = 360f / m_projectileCount;
            for (int i = 0; i < m_projectileCount; i++)
            {
                var rotation = rotationInterval * i;
                var RadRotation = Mathf.Deg2Rad * rotation;
                var angleInVector2D = new Vector3(Mathf.Cos(RadRotation), Mathf.Sin(RadRotation), 0);
                var position = transform.position + (m_distanceFromCenter * angleInVector2D);
                Gizmos.DrawCube(position, Vector3.one * 2.5f);
            }
        }
    }
}