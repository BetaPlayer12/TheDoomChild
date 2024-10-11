using UnityEngine;
using System.Collections.Generic;
using DChild.Gameplay.Projectiles;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusRainProjectileHandle
    {
        private float m_minimalDistancePerInstance;
        private List<Projectile> m_spawnedProjectiles;

        private List<Vector2> m_spawnedPosition;
        private const int MAXPOSITIONGENERATIONATTEMPT = 100;

        public PuedisYnnusRainProjectileHandle(float minimalDistancePerInstance)
        {
            m_minimalDistancePerInstance = minimalDistancePerInstance;
            m_spawnedProjectiles = new List<Projectile>();
            m_spawnedPosition = new List<Vector2>();
        }

        public void SpawnProjectiles(GameObject projectile, Bounds spawnBounds, int amountToSpawn)
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                int attempts = 0;
                Vector2 position = Vector2.zero;
                do
                {
                    position = GenerateRandomPositionInBounds(spawnBounds);
                    attempts++;

                } while (IsTooCloseToSpawnedProjectiles(position) && attempts < MAXPOSITIONGENERATIONATTEMPT);

                if (IsTooCloseToSpawnedProjectiles(position) && attempts >= MAXPOSITIONGENERATIONATTEMPT)
                {
                    Debug.LogWarning($"Cannot Spawn {amountToSpawn - i} more Objects as it is too crowded");
                    break;
                }

                var projecitle = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile, position, Quaternion.Euler(Vector3.down));
                m_spawnedProjectiles.Add(projecitle);
                m_spawnedPosition.Add(position);
            }
        }

        public void DropSpawnedProjectiles(float speed)
        {
            for (int i = 0; i < m_spawnedProjectiles.Count; i++)
            {
                var rigidbody = m_spawnedProjectiles[i].GetComponent<Rigidbody2D>();
                rigidbody.velocity = Vector2.down * speed;
            }
            m_spawnedProjectiles.Clear();
            m_spawnedPosition.Clear();
        }

        private bool IsTooCloseToSpawnedProjectiles(Vector2 position)
        {
            for (int i = 0; i < m_spawnedPosition.Count; i++)
            {
                if (Vector2.Distance(position, m_spawnedPosition[i]) < m_minimalDistancePerInstance)
                    return true;
            }

            return false;
        }

        private Vector2 GenerateRandomPositionInBounds(Bounds bounds)
        {
            var extents = bounds.extents;
            var position = bounds.center;
            position.x += Random.Range(-extents.x, extents.x);
            position.y += Random.Range(-extents.y, extents.y);
            return position;
        }
    }
}