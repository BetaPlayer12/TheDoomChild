using UnityEngine;
using System.Collections;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    public class DemonLordIceShardSpell : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_projectile;
        [SerializeField]
        private Vector2 m_castDimension;
        [SerializeField]
        private float m_shardDelayedLaunch;

        public void LaunchIceShards(Vector3 position, float rotation, float speed)
        {
            StartCoroutine(LaunchIceShardsRoutine(position, rotation, speed));
        }

        private IEnumerator LaunchIceShardsRoutine(Vector3 position, float rotation, float speed)
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            yield return null;
            Projectile[] projectiles = null;
            yield return SpawnProjectilesInFormationRoutine(projectiles);
            Debug.Log(projectiles.ToString());
            yield return new WaitForSeconds(m_shardDelayedLaunch);
            LaunchProjectiles(projectiles, speed);
        }

        private IEnumerator SpawnProjectilesInFormationRoutine(Projectile[] projectiles)
        {
            var castDimensionExtent = m_castDimension / 2;
            var centerPosition = transform.position;

            projectiles = new Projectile[4];

            projectiles[0] = SpawnProjectile((transform.right * castDimensionExtent.x), 0);
            projectiles[1] = SpawnProjectile((transform.up * castDimensionExtent.y), 90);
            projectiles[2] = SpawnProjectile((-transform.right * castDimensionExtent.x), 180);
            projectiles[3] = SpawnProjectile((-transform.up * castDimensionExtent.y), 270);

            yield return null;
        }

        private Projectile SpawnProjectile(Vector3 position, float rotation)
        {
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectile);
            instance.transform.parent = transform;
            instance.transform.localPosition = position;
            instance.transform.localRotation = Quaternion.Euler(0, 0, rotation);
            instance.transform.parent = null;
            return instance.GetComponent<Projectile>();
        }

        private void LaunchProjectiles(Projectile[] projectiles, float speed)
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                var projectile = projectiles[i];
                projectile.SetVelocity(projectile.transform.right, speed);
            }
        }

        private void OnDrawGizmosSelected()
        {
            var castDimensionExtent = m_castDimension / 2;
            var centerPosition = transform.position;

            var rightPos = centerPosition + (transform.right * castDimensionExtent.x);
            var upPos = centerPosition + (transform.up * castDimensionExtent.y);
            var leftPos = centerPosition + (-transform.right * castDimensionExtent.x);
            var downPos = centerPosition + (-transform.up * castDimensionExtent.y);

            var cubeSize = Vector3.one * 2.5f;
            Gizmos.DrawCube(rightPos, cubeSize);
            Gizmos.DrawCube(upPos, cubeSize);
            Gizmos.DrawCube(leftPos, cubeSize);
            Gizmos.DrawCube(downPos, cubeSize);

            Gizmos.DrawLine(rightPos, upPos);
            Gizmos.DrawLine(rightPos, downPos);
            Gizmos.DrawLine(leftPos, upPos);
            Gizmos.DrawLine(leftPos, downPos);
        }
    }
}