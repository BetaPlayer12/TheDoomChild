using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{

    public class BlackDeathSoundMark : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem m_anticipationFX;
        [SerializeField]
        private ParticleSystem m_fx;
        [SerializeField]
        private float m_fxDelay;
        [SerializeField]
        private Collider2D m_damageCollider;
        [SerializeField]
        private float m_damageColliderDelay;
        [SerializeField]
        private float m_damageColliderDuration;

        [SerializeField]
        private GameObject m_projectile;
        [SerializeField, Min(0)]
        private float m_projectileSpeed;
        [SerializeField]
        private Transform m_projectileSpawnPoint;
        [SerializeField]
        private float m_projectileSpawnDelay;

        private bool m_isActivated;
        public bool isActivated => m_isActivated;

        [Button]
        public void Activate(Transform target)
        {
            if (m_isActivated)
                return;
            StartCoroutine(ActivationRoutine());
            StartCoroutine(DelayedSpawnProjectileRoutine(target));
        }

        private IEnumerator ActivationRoutine()
        {
            m_isActivated = true;
            m_anticipationFX.Play(true);
            yield return new WaitForSeconds(m_fxDelay);
            m_fx.Play(true);
            yield return new WaitForSeconds(m_damageColliderDelay);
            m_damageCollider.enabled = true;
            yield return new WaitForSeconds(m_damageColliderDuration);
            m_damageCollider.enabled = false;
            while (m_fx.isPlaying)
                yield return null;

            m_isActivated = false;
        }

        private IEnumerator DelayedSpawnProjectileRoutine(Transform target)
        {
            yield return new WaitForSeconds(m_projectileSpawnDelay);
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_projectile);
            instance.ResetState();
            instance.SpawnAt(m_projectileSpawnPoint.position, Quaternion.identity);
            instance.Launch(Vector2.down, m_projectileSpeed);
            instance.GetComponent<BlackDeathSoundMarkProjectile>().HomeTowards(target);
        }

        private void Awake()
        {
            m_damageCollider.enabled = false;
        }
    }

}