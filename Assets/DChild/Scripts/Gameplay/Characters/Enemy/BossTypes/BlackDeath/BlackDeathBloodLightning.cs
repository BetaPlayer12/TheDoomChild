using System.Collections;
using System.Collections.Generic;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BlackDeathBloodLightning : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_hurtBoxCollider;
        [SerializeField]
        private float m_timeToWait;
        [SerializeField]
        private float m_timeToActivateHurtbox;
        [SerializeField]
        private float m_timeToDeactivateHurtbox;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_indicatorFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_lightningFX;

        private bool m_isSpawning;

        public EventAction<EventActionArgs> IsDone;

        [Button, HideInEditorMode]
        public void Execute()
        {
            if (m_isSpawning)
                return;

            StopAllCoroutines();
            StartCoroutine(HurtboxRoutine());
        }

        private IEnumerator HurtboxRoutine()
        {
            m_isSpawning = true;

            m_indicatorFX.Play();
            yield return new WaitForSeconds(m_timeToWait);
            m_lightningFX.Play();
            yield return new WaitForSeconds(m_timeToActivateHurtbox);
            m_hurtBoxCollider.enabled = true;
            yield return new WaitForSeconds(m_timeToDeactivateHurtbox);
            m_hurtBoxCollider.enabled = false;
            yield return new WaitForSeconds(.5f);
            Destroy(this.gameObject);
            yield return null;

            m_isSpawning = false;
            IsDone?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_hurtBoxCollider.enabled = false;
        }
    }

}