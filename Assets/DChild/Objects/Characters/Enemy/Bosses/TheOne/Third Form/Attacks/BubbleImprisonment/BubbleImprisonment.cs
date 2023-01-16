using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;


namespace DChild.Gameplay.Characters.Enemies
{
    public class BubbleImprisonment : PoolableObject
    {
        [SerializeField]
        private Collider2D m_trapCollider;
        [SerializeField]
        private Collider2D m_damageCollider;
        public float timeBetweenAnimations;

        [SerializeField]
        private Animator m_animatorPool;
        [SerializeField]
        private Animator m_animatorImplode;
        [SerializeField]
        private ParticleSystem m_particlePool;
        [SerializeField]
        private ParticleSystem m_particleExplode;

        private void Start()
        {
            m_trapCollider.enabled = false;
            m_damageCollider.enabled = false;
            StartCoroutine(BubbleImprisonmentSequence());
        }

        private IEnumerator BubbleImprisonmentSequence()
        {
            yield return BubbleEmerge();
            yield return BubbleExplode();
            yield return BubbleDespawn();
        }

        private IEnumerator BubbleEmerge()
        {
            m_animatorPool.SetInteger("BubState", 1);
            yield return new WaitForSeconds(timeBetweenAnimations);
            m_animatorImplode.SetInteger("BubState", 1);
            m_trapCollider.enabled = true;
        }

        private IEnumerator BubbleExplode()
        {
            yield return new WaitForSeconds(timeBetweenAnimations);
            m_animatorImplode.SetInteger("BubState", 2);
            m_damageCollider.enabled = true;
            m_animatorPool.SetInteger("BubState", 2);
        }

        private IEnumerator BubbleDespawn()
        {
            m_particleExplode.Play();
            DestroyInstance();
            yield return null;
        }
    }
}

