using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Projectiles
{
    public class StormCloud : PoolableObject
    {
        [SerializeField]
        private float m_timer;
        [SerializeField]
        private Animator m_animator;
        public Animator animator => m_animator;
        [SerializeField]
        private ParticleSystem m_particle;
        [SerializeField]
        private Collider2D m_playerDetection;
        [SerializeField]
        private Collider2D m_damageCollider;
        public GameObject m_tomeOfSpellsStorm;

        private Coroutine m_summonStormCloudRoutine;



        public EventAction<EventActionArgs> OnDestroyedInstance;
        // Start is called before the first frame update
        void Start()
        {
            m_animator.SetTrigger("StartTrigger");
            m_summonStormCloudRoutine = StartCoroutine(SummonStormCloudRoutine());
            m_tomeOfSpellsStorm.GetComponent<Damageable>().Destroyed += OnTomeDeath;
            //CallPoolRequest();
        }

        private void OnTomeDeath(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            Debug.Log("On Tome Death Event");
            StartCoroutine(TomeDeathRoutine());
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private IEnumerator SummonStormCloudRoutine()
        {
            while (m_timer > 0)
            {
                yield return new WaitForSeconds(1f);
                m_timer--;
                yield return null;
            }
            m_animator.SetTrigger("AnticipationEnd");
            m_damageCollider.enabled = false;
            yield return new WaitForSeconds(3f);
            DestroyInstance();
            OnDestroyedInstance?.Invoke(this, EventActionArgs.Empty);
            yield return null;
        }

        private IEnumerator StormCloudTriggered()
        {
            m_damageCollider.enabled = true;
            yield return new WaitForSeconds(2f);
            m_animator.SetTrigger("End");
            m_damageCollider.enabled = false;
            yield return new WaitForSeconds(2f);
            DestroyInstance();
            OnDestroyedInstance?.Invoke(this, EventActionArgs.Empty);
            yield return null;
        }

        private IEnumerator TomeDeathRoutine()
        {
            if (m_animator.GetBool("AttackTr2igger") == true)
            {
                m_animator.SetTrigger("End");
                Debug.Log("Attack");
            }
            else
            {
                m_animator.SetTrigger("AnticipationEnd");
                Debug.Log("Not Attack");
            }
            yield return new WaitForSeconds(2.5f);
            DestroyInstance();
            yield return null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
         {
            if (collision.gameObject.layer == 8)
             {
                StopCoroutine(m_summonStormCloudRoutine);
                m_playerDetection.enabled = false;
                m_animator.SetTrigger("AttackTrigger");
                StartCoroutine(StormCloudTriggered());
            }
        }
    }

}