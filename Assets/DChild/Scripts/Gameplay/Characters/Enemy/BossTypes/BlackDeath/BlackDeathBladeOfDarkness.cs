using Holysoft.Event;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System;

namespace DChild.Gameplay.Characters.Enemies
{

    public class BlackDeathBladeOfDarkness : MonoBehaviour
    {
        [SerializeField]
        private Animator m_bladeAnimator;
        [SerializeField]
        private ParticleSystem m_anitcipationFX;
        [SerializeField]
        private ParticleSystem m_indicatorFX;
        [SerializeField]
        private ParticleSystem m_spawnDebrisFX;

        [SerializeField, Min(0)]
        private float m_anticipationDuration;
        [SerializeField, Min(0)]
        private float m_indicatorDuration;
        [SerializeField, Min(0)]
        private float m_spawnDuration;

        private int m_bladeSpawnParamID;
        private int m_bladeRetractParamID;
        private bool m_isSpawning;

        public event EventAction<EventActionArgs> IsDone;

        [Button, HideInEditorMode]
        public void Execute()
        {
            if (m_isSpawning)
                return;

            StopAllCoroutines();
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            m_isSpawning = true;

            m_anitcipationFX.Play(true);
            yield return new WaitForSeconds(m_anticipationDuration);
            m_anitcipationFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            m_indicatorFX.Play(true);
            yield return new WaitForSeconds(m_indicatorDuration);
            m_indicatorFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            m_bladeAnimator.SetTrigger(m_bladeSpawnParamID);
            m_spawnDebrisFX.Play(true);
            yield return new WaitForSeconds(m_spawnDuration);
            m_bladeAnimator.SetTrigger(m_bladeRetractParamID);

            m_isSpawning = false;
            IsDone?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_bladeSpawnParamID = Animator.StringToHash("Spawn");
            m_bladeRetractParamID = Animator.StringToHash("Retract");
        }
    }
}