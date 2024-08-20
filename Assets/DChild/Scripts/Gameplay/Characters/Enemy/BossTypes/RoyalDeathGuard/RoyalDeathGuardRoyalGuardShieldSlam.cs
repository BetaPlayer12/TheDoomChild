using UnityEngine;
using System.Collections;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardRoyalGuardShieldSlam : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_projectile;
        [SerializeField, BoxGroup("Spawn")]
        private Transform m_spawnPoint;
        [SerializeField, BoxGroup("Spawn")]
        private float m_groundPoint;
        [SerializeField, Min(1), BoxGroup("Spawn")]
        private int m_spawnCount = 1;
        [SerializeField, Min(0), BoxGroup("Spawn")]
        private float m_spawnInterval;

        [SerializeField, Min(0.1f), BoxGroup("Projectile")]
        private float m_positioningDuration;
        [SerializeField, Min(0), BoxGroup("Projectile")]
        private float m_projectileLaunchDelay;
        [SerializeField, Min(0), BoxGroup("Projectile")]
        private float m_projectileSpeed;


        private bool m_isExecuting;
        public event EventAction<EventActionArgs> ProjectileSpawnDone;

        [Button]
        public void Execute(Transform target)
        {
            if (m_isExecuting)
                return;

            m_isExecuting = true;
            StartCoroutine(SpawnProjectiles(target, m_spawnCount));
        }

        private IEnumerator SpawnProjectiles(Transform target, int count)
        {
            m_isExecuting = true;
            for (int i = 0; i < count; i++)
            {
                StartCoroutine(SpawnProjectile(target));
                if (i + 1 < count)
                {
                    yield return new WaitForSeconds(m_spawnInterval);
                }
            }
            ProjectileSpawnDone?.Invoke(this, EventActionArgs.Empty);
            m_isExecuting = false;
        }

        private IEnumerator SpawnProjectile(Transform target)
        {
            var spawnPosition = m_spawnPoint.position;
            spawnPosition.y = m_groundPoint;
            var projectile = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectile, m_spawnPoint.gameObject.scene, spawnPosition, Quaternion.identity);
            var scaleDirection = Mathf.Sign(transform.lossyScale.x);
            var projectileScale = projectile.transform.localScale;
            projectileScale.x *= scaleDirection;
            projectile.transform.localScale = projectileScale;
            yield return GetToPosition(projectile.transform, transform.position);
            var deathStenchProjectile = (RoyalDeathGuardDeathStenchProjectile)projectile;
            if (deathStenchProjectile != null)
            {
                yield return new WaitForSeconds(m_projectileLaunchDelay);
                deathStenchProjectile.Release(target, m_projectileSpeed);
            }
        }

        private IEnumerator GetToPosition(Transform toMove, Vector3 position)
        {
            var startPosition = toMove.position;
            var time = 0f;
            var deltaModifier = 1 / m_positioningDuration;
            do
            {
                toMove.position = Vector3.Lerp(startPosition, position, time);
                time += GameplaySystem.time.deltaTime * deltaModifier;
                yield return null;
            } while (time < 1);
        }
    }
}