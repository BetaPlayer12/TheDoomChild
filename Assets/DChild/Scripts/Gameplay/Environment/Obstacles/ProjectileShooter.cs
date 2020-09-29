using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class ProjectileShooter : MonoBehaviour
    {
        [SerializeField]
        private ProjectileInfo m_toShoot;
        [SerializeField]
        private float m_offset;
        [SerializeField]
        private GameObject m_shootFX;
        [SerializeField, MinValue(0)]
        private float m_fireDelay;
        [SerializeField]
        private GameObject m_preShootFX;

        private Coroutine m_routine;
        private ProjectileLaunchHandle m_launcher;
        private FXSpawnHandle<FX> m_FXSpawner;

        public float fireDelay => m_fireDelay;

        [Button]
        public void Shoot()
        {
            if (m_routine == null)
            {
                m_routine = StartCoroutine(DelayShootRoutine());
            }
        }

        private IEnumerator DelayShootRoutine()
        {
            if (m_fireDelay > 0)
            {
                if (m_shootFX == null)
                {
                    yield return new WaitForSeconds(m_fireDelay);
                }
                else
                {
                    var preShootFX = m_FXSpawner.InstantiateFX(m_preShootFX, transform.position);
                    preShootFX.transform.parent = transform;
                    yield return new WaitForSeconds(m_fireDelay);
                    preShootFX.Stop();
                }
            }
            var position = transform.position + transform.right * m_offset;
            m_launcher.Launch(m_toShoot.projectile, position, transform.right, m_toShoot.speed);
            if (m_shootFX != null)
            {
                m_FXSpawner.InstantiateFX(m_shootFX, transform.position);
            }
            m_routine = null;
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            ContactFilter2D filter = new ContactFilter2D();
            filter.useLayerMask = true;
            if (m_toShoot.projectile != null)
            {
                filter.SetLayerMask(Physics2D.GetLayerCollisionMask(m_toShoot.projectile.layer));
            }
            else
            {
                filter.SetLayerMask(LayerMask.GetMask("Environment"));
            }
            filter.useTriggers = false;
            RaycastHit2D[] hitbuffers = new RaycastHit2D[16];
            var position = transform.position + transform.right * m_offset;
            var hits = Physics2D.Raycast(position, transform.right, filter, hitbuffers);
            Gizmos.color = Color.red;
            if (hits > 0)
            {
                RaycastHit2D hit = hitbuffers[0];
                var toHit = hit.point - (Vector2)position;
                Gizmos.DrawRay(position, toHit);
            }
            else
            {
                Gizmos.DrawRay(position, transform.right * 1000f);
            }
#endif
        }
    }
}
