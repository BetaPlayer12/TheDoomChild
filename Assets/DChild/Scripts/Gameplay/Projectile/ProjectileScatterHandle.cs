using DChild.Gameplay.Characters;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DChild.Gameplay.Projectiles
{
    public class ProjectileScatterHandle : MonoBehaviour
    {
        private struct SpawnConfiguration
        {
            public SpawnConfiguration(Vector2 position, Vector2 direction)
            {
                this.position = position;
                this.direction = direction;
            }

            public Vector2 position { get; }

            public Vector2 direction { get; }
        }

        private enum ScatterType
        {
            Arc,
            Circle
        }

        [SerializeField]
        private GameObject m_projectile;
        [SerializeField, Min(2)]
        private int m_projectileCount = 2;
        [SerializeField, Min(0)]
        private float m_projectileSpeed;
        [SerializeField, Min(0)]
        private float m_fromCenterOffset;
        [SerializeField, Min(0)]
        private float m_interval;
        [SerializeField]
        private ScatterType m_type;
        [SerializeField, Wrap(1, 360), ShowIf("@m_type == ScatterType.Arc")]
        private int m_scatterAngle = 90;

        private List<Projectile> m_spawnedProjectiles;

        [Button, HideInEditorMode]
        public void SpawnProjectiles()
        {
            var configurations = CalculateConfiguration();
            for (int i = 0; i < m_projectileCount; i++)
            {
                SpawnProjectile(configurations[i]);
            }
        }

        [Button, HideInEditorMode]
        public void SpawnProjectileInSequence(HorizontalDirection direction, bool launchImmidiately)
        {
            StopAllCoroutines();
            StartCoroutine(SpawnProjectileInSequenceRoutine(direction, launchImmidiately));
        }

        private IEnumerator SpawnProjectileInSequenceRoutine(HorizontalDirection direction, bool launchImmidiately)
        {
            var configurations = CalculateConfiguration();
            if (direction == HorizontalDirection.Right)
            {
                for (int i = 0; i < m_projectileCount; i++)
                {
                    SpawnProjectile(configurations[i]);
                    if (launchImmidiately)
                    {
                        var projectile = m_spawnedProjectiles[i];
                        m_spawnedProjectiles[i].Launch(projectile.transform.right, m_projectileSpeed);
                    }
                    yield return new WaitForSeconds(m_interval);
                }
            }
            else
            {
                var spawnIndex = 0;
                for (int i = m_projectileCount- 1; i >= 0; i--)
                {
                    SpawnProjectile(configurations[i]);
                    if (launchImmidiately)
                    {
                        var projectile = m_spawnedProjectiles[spawnIndex];
                        m_spawnedProjectiles[spawnIndex].Launch(projectile.transform.right, m_projectileSpeed);
                    }
                    yield return new WaitForSeconds(m_interval);
                    spawnIndex++;
                }
            }

            if (launchImmidiately)
            {
                m_spawnedProjectiles.Clear();
            }
        }

        [Button, HideInEditorMode]
        public void LaunchSpawnedProjectiles()
        {
            for (int i = 0; i < m_spawnedProjectiles.Count; i++)
            {
                var projectile = m_spawnedProjectiles[i];
                m_spawnedProjectiles[i].Launch(projectile.transform.right, m_projectileSpeed);
            }
            m_spawnedProjectiles.Clear();
        }

        private void SpawnProjectile(SpawnConfiguration configuration)
        {
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectile);
            instance.transform.position = configuration.position;
            var component = instance.GetComponent<Projectile>();
            component.ResetState();
            instance.transform.right = configuration.direction;
            m_spawnedProjectiles.Add(component);
        }

        private SpawnConfiguration[] CalculateConfiguration()
        {
            var configuration = new SpawnConfiguration[m_projectileCount];

            if (m_projectileCount < 2)
            {
                throw new System.Exception("Projectile Scatter Handle Does not support Projectile count less than 2");
            }
            else
            {
                switch (m_type)
                {
                    case ScatterType.Arc:
                        CalculateArcConfiguration(configuration);
                        break;
                    case ScatterType.Circle:
                        CalculateCircleConfiguration(configuration);
                        break;
                }
            }
            return configuration;
        }

        private void CalculateArcConfiguration(SpawnConfiguration[] configuration)
        {
            float lerpInterval = 1f / (m_projectileCount - 1);
            var angleExtent = m_scatterAngle / 2f;
            float lerpValue = 0f;
            for (int i = 0; i < m_projectileCount; i++)
            {
                var angle = Mathf.Lerp(-angleExtent, angleExtent, lerpValue);
                var direction = GetVectorDirection(angle);

                configuration[i] = new SpawnConfiguration(CalculateOffsetSpawnPosition(direction), direction);
                lerpValue += lerpInterval;
            }
        }

        private void CalculateCircleConfiguration(SpawnConfiguration[] configuration)
        {
            float angleStep = 360f / m_projectileCount;
            float angle = 0;

            for (int i = 0; i < m_projectileCount; i++)
            {
                var angleVector = GetVectorDirection(angle);

                configuration[i] = new SpawnConfiguration(CalculateOffsetSpawnPosition(angleVector), angleVector);
                angle += angleStep;
            }
        }

        private Vector3 GetVectorDirection(float angle)
        {
            float relativeScatterAngle = GetRelativeScatterAngle(angle) * Mathf.Deg2Rad;
            var angleVector = new Vector3(Mathf.Sin(relativeScatterAngle), Mathf.Cos(relativeScatterAngle));
            return angleVector;
        }

        private float GetRelativeScatterAngle(float angle)
        {
            var baseTransformRightAngle = transform.rotation.eulerAngles.z - 90;
            var relativeScatterAngle = Mathf.Repeat((angle - baseTransformRightAngle), 360);
            return relativeScatterAngle;
        }

        private Vector3 CalculateOffsetSpawnPosition(Vector3 direction)
        {
            return transform.position + (direction * m_fromCenterOffset);
        }

        private void Awake()
        {
            m_spawnedProjectiles = new List<Projectile>();
        }

        private void OnDrawGizmosSelected()
        {
            var configuration = CalculateConfiguration();

            switch (m_type)
            {
                case ScatterType.Arc:
                    DrawArcGizmos(configuration);
                    break;
                case ScatterType.Circle:
                    break;
            }

            Gizmos.color = Color.white;
            for (int i = 0; i < configuration.Length; i++)
            {
                var config = configuration[i];
                Vector2 endPoint = config.position + (config.direction * 10f);
                Gizmos.DrawLine(config.position, endPoint);
                Gizmos.DrawCube(endPoint, Vector3.one * 0.25f);
            }
        }

        private void DrawArcGizmos(SpawnConfiguration[] configuration)
        {
            Gizmos.color = Color.yellow;
            var baseLineEndpoint = transform.position + (transform.right * 1);
            Gizmos.DrawLine(transform.position, baseLineEndpoint);
            Gizmos.DrawCube(baseLineEndpoint, Vector3.one * 0.35f);

            Gizmos.color = Color.red;
            var angleExtent = m_scatterAngle / 2f;
            DrawScatterBoarder(-angleExtent);
            DrawScatterBoarder(angleExtent);

            void DrawScatterBoarder(float angle)
            {
                var angleVector = GetVectorDirection(angle);
                var angleVectorEndPoint = transform.position + (angleVector * 20);
                Gizmos.DrawLine(transform.position, angleVectorEndPoint);
                Gizmos.DrawCube(angleVectorEndPoint, Vector3.one * 0.35f);
            }
        }
    }
}

