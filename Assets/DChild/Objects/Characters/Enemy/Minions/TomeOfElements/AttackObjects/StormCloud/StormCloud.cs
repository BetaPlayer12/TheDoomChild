using DChild.Gameplay.Pooling;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class StormCloud : PoolableObject
    {
        [SerializeField]
        private float m_timer;
        [SerializeField]
        private Animator m_animator;
        [SerializeField]
        private ParticleSystem m_particle;
        [SerializeField]
        private Collider2D m_playerDetection;
        [SerializeField]
        private Collider2D m_damageCollider;

        private Coroutine m_summonStormCloudRoutine;

        public EventAction<EventActionArgs> OnDestroyedInstance;
        // Start is called before the first frame update
        void Start()
        {
            m_animator.SetTrigger("StartTrigger");
            m_summonStormCloudRoutine = StartCoroutine(SummonStormCloudRoutine());
            //CallPoolRequest();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private IEnumerator SummonStormCloudRoutine()
        {
            yield return new WaitForSeconds(1f);
            while (m_timer > 0)
            {
                yield return new WaitForSeconds(1f);
                m_timer--;
            }
            m_animator.SetTrigger("End");
            m_damageCollider.enabled = false;
            yield return new WaitForSeconds(3f);
            DestroyInstance();
            OnDestroyedInstance?.Invoke(this, EventActionArgs.Empty);
        }

        private IEnumerator StormCloudTriggered()
        {
            m_damageCollider.enabled = true;
            yield return new WaitForSeconds(1f);
            m_animator.SetTrigger("End");
            m_damageCollider.enabled = false;
            yield return new WaitForSeconds(3f);
            m_damageCollider.enabled = false;
            DestroyInstance();
            OnDestroyedInstance?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnTriggerEnter2D(Collider2D collision)
         {
            if (collision.gameObject.layer == 8)
            {
                m_playerDetection.enabled = false;
                m_animator.SetTrigger("AttackTrigger");
                StartCoroutine(StormCloudTriggered());
                StopCoroutine(m_summonStormCloudRoutine);
            }
        }
    }

}