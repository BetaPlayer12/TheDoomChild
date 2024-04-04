using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Pooling
{
    public class GeyserBlast : PoolableObject
    {
        [SerializeField]
        private Collider2D m_geyserCollider;

        [SerializeField]
        private Collider2D m_poolCollider;

        [SerializeField]
        private float m_puddleDuration = 10f;

        [SerializeField]
        private ParticleSystem m_geyserParticles;

        private void Start()
        {
            m_geyserCollider.enabled = false;
            m_poolCollider.enabled = true;

            StartCoroutine(GeyserColliderRoutine());
        }

        private IEnumerator GeyserColliderRoutine()
        {
            var m_geyserDelay = m_geyserParticles.startDelay;

            var m_geyserDuration = m_geyserParticles.duration;

            yield return new WaitForSeconds(m_geyserDelay);

            m_geyserCollider.enabled = true;

            yield return new WaitForSeconds(m_geyserDuration);

            m_geyserCollider.enabled = false;
        }

        private void Update()
        {
            if(m_puddleDuration > 0)
            {
                m_puddleDuration -= Time.deltaTime;
            }
            else
            {
                DestroyInstance();
            }
        }
    }
}


